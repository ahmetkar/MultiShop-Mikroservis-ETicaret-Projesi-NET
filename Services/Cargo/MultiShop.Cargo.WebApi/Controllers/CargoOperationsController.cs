using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BussinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoOperation;
using MultiShop.Cargo.EntityLayer.Concretes;
using MultiShop.SharedLayer.Events;
using MultiShop.SharedLayer.Kafka;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOperationsController : ControllerBase
    {
        private readonly ICargoOperationService _CargoOperationsService;
        private readonly IKafkaProducer _kafkaProducer;

        public CargoOperationsController(ICargoOperationService CargoOperationsService,IKafkaProducer kafkaProducer)
        {
            _CargoOperationsService = CargoOperationsService;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet]
        public IActionResult CargoOperationsList()
        {
            var result = _CargoOperationsService.TGetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoOperationById(int id)
        {
            var result = _CargoOperationsService.TGetById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCargoOperations(CreateCargoOperationDto createCargoOperationDto)
        {
            CargoOperation CargoOperation = new CargoOperation
            {
               CargoDetailId = createCargoOperationDto.CargoDetailId,
               Description = createCargoOperationDto.Description,
                OperationDate = createCargoOperationDto.OperationDate,
            };

            _CargoOperationsService.TInsert(CargoOperation);
            return Ok("Kargo operasyonu başarıyla oluşturuldu");
        }

        [HttpDelete]
        public IActionResult RemoveCargoOperations(int id)
        {
            _CargoOperationsService.TDelete(id);
            return Ok("Kargo operasyonu başarıyla silindi");
        }

        [HttpPut]
        public IActionResult UpdateCargoOperations(UpdateCargoOperationDto updateCargoOperationDto)
        {
            CargoOperation CargoOperation = new CargoOperation
            {
                CargoOperationId = updateCargoOperationDto.CargoOperationId,
                CargoDetailId = updateCargoOperationDto.CargoDetailId,
                Description = updateCargoOperationDto.Description,
                OperationDate = updateCargoOperationDto.OperationDate,
            };
            _CargoOperationsService.TUpdate(CargoOperation);
            return Ok("Kargo operasyonu başarıyla güncellendi");
        }

        [HttpPut("SetDelivered")]
        public IActionResult SetDeliveredCargoOperation(UpdateCargoOperationDto updateCargoOperationDto,CancellationToken cancellationToken)
        {
            try
            {
                CargoOperation CargoOperation = new CargoOperation
                {
                    CargoOperationId = updateCargoOperationDto.CargoOperationId,
                    CargoDetailId = updateCargoOperationDto.CargoDetailId,
                    Description = updateCargoOperationDto.Description,
                    OperationDate = updateCargoOperationDto.OperationDate,
                    OrderingId = updateCargoOperationDto.OrderingId,
                    IsCompleted = true,
                };
                _CargoOperationsService.TUpdate(CargoOperation);

                var cargoDelivered = new CargoOperationDelivered
                {
                    CargoDetailId = CargoOperation.CargoDetailId,
                    CargoOperationId = CargoOperation.CargoOperationId,
                    OperationDate = CargoOperation.OperationDate,
                    OrderingId = CargoOperation.OrderingId,

                };

                _kafkaProducer.PublishAsync(KafkaTopics.CargoDelivered,cargoDelivered,CargoOperation.CargoOperationId.ToString(),cancellationToken);



                return Ok(new { success=true,message = "Kargo başarıyla oluşturuldu" });

            }
            catch (Exception ex) {
                return Ok(new { success = false, message = ex.Message });
            }
            
        }

        [HttpGet("ConfirmDelivery/{id}")]
        [HttpPost("ConfirmDelivery/{id}")]
        public IActionResult ConfirmDelivery(int id)
        {
            try
            {
                var operation = _CargoOperationsService.TGetById(id);
                if (operation != null)
                {
                    operation.IsCompleted = true;
                    operation.Description = "Kargo teslim edildi.";
                    operation.OperationDate = DateTime.Now;
                    _CargoOperationsService.TUpdate(operation);
                    return Ok(new { success = true, message = "Kargo teslim edildi olarak onaylandı." });
                }
                return NotFound("Kargo operasyonu bulunamadı.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
