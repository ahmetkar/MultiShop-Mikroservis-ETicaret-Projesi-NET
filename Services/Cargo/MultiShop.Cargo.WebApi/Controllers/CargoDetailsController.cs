using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BussinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoDetail;
using MultiShop.Cargo.EntityLayer.Concretes;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoDetailsController : ControllerBase
    {
        private readonly ICargoDetailService _CargoDetailsService;

        public CargoDetailsController(ICargoDetailService CargoDetailsService)
        {
            _CargoDetailsService = CargoDetailsService;
        }

        [HttpGet]
        public IActionResult CargoDetailsList()
        {
            var result = _CargoDetailsService.TGetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoDetailsById(int id)
        {
            var result = _CargoDetailsService.TGetById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCargoDetails(CreateCargoDetailDto createCargoDetailsDto)
        {
            CargoDetail CargoDetails = new CargoDetail
            {
                SenderCustomer = createCargoDetailsDto.SenderCustomer,
                ReceiverCustomer = createCargoDetailsDto.ReceiverCustomer,
                Barcode = createCargoDetailsDto.Barcode,
                CargoCompanyId = createCargoDetailsDto.CargoCompanyId,
            };

            _CargoDetailsService.TInsert(CargoDetails);
            return Ok("Kargo detayları başarıyla oluşturuldu");
        }

        [HttpDelete]
        public IActionResult RemoveCargoDetails(int id)
        {
            _CargoDetailsService.TDelete(id);
            return Ok("Kargo detayları başarıyla silindi");
        }

        [HttpPut]
        public IActionResult UpdateCargoDetails(UpdateCargoDetailDto updateCargoDetailsDto)
        {
            CargoDetail CargoDetails = new CargoDetail
            {
                CargoDetailId = updateCargoDetailsDto.CargoDetailId,
                SenderCustomer = updateCargoDetailsDto.SenderCustomer,
                ReceiverCustomer = updateCargoDetailsDto.ReceiverCustomer,
                Barcode = updateCargoDetailsDto.Barcode,
                CargoCompanyId = updateCargoDetailsDto.CargoCompanyId,
            };
            _CargoDetailsService.TUpdate(CargoDetails);
            return Ok("Kargo detayları başarıyla güncellendi");
        }

    }
}
