using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BussinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoOperation;
using MultiShop.Cargo.EntityLayer.Concretes;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOperationsController : ControllerBase
    {
        private readonly ICargoOperationService _CargoOperationsService;

        public CargoOperationsController(ICargoOperationService CargoOperationsService)
        {
            _CargoOperationsService = CargoOperationsService;
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
               Barcode = createCargoOperationDto.Barcode,
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
                Barcode = updateCargoOperationDto.Barcode,
                Description = updateCargoOperationDto.Description,
                OperationDate = updateCargoOperationDto.OperationDate,
            };
            _CargoOperationsService.TUpdate(CargoOperation);
            return Ok("Kargo operasyonu başarıyla güncellendi");
        }
    }
}
