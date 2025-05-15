using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Commands.AdressCommands;
using MultiShop.Order.Application.Features.CQRS.Handlers.AdressHandlers;
using MultiShop.Order.Application.Features.CQRS.Queries.AdressQueries;

namespace MultiShop.Order.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly GetAdressQueryHandler _getAdressQueryHandler;
        private readonly GetAdressByIdQueryHandler _getAdressByIdQueryHandler;
        private readonly CreateAdressCommandHandler _createAdressCommandHandler;
        private readonly UpdateAdressCommandHandler _updateAdressCommandHandler;
        private readonly RemoveAdressCommandHandler _removeAdressCommandHandler;

        public AdressesController(GetAdressQueryHandler getAdressQueryHandler, GetAdressByIdQueryHandler getAdressByIdQueryHandler, CreateAdressCommandHandler createAdressCommandHandler,
            UpdateAdressCommandHandler updateAdressCommandHandler, RemoveAdressCommandHandler deleteAdressCommandHandler)
        {
            _getAdressQueryHandler = getAdressQueryHandler;
            _getAdressByIdQueryHandler = getAdressByIdQueryHandler;
            _createAdressCommandHandler = createAdressCommandHandler;
            _updateAdressCommandHandler = updateAdressCommandHandler;
            _removeAdressCommandHandler = deleteAdressCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> AddressList()
        {
            var values = await _getAdressQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdressById(int id)
        {
            var value = await _getAdressByIdQueryHandler.Handle(new GetAdressByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdress(CreateAdressCommand createAdressCommand)
        {
            await _createAdressCommandHandler.Handle(createAdressCommand);
            return Ok("Adres başarıyla oluşturuldu");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdress(UpdateAdressCommand updateAdressCommand)
        {
            await _updateAdressCommandHandler.Handle(updateAdressCommand);
            return Ok("Adres başarıyla güncellendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAdress(int id)
        {
            await _removeAdressCommandHandler.Handle(new RemoveAdressCommand(id));
            return Ok("Adres başarıyla silindi.");
        }


        }
}
