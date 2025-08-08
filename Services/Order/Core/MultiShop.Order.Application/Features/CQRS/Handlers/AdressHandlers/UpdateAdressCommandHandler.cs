using MultiShop.Order.Application.Features.CQRS.Commands.AdressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AdressHandlers
{
    public class UpdateAdressCommandHandler
    {
        private readonly IRepository<Adress> _repository;

        public UpdateAdressCommandHandler(IRepository<Adress> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateAdressCommand command)
        {
           var values = await _repository.GetByIdAsync(command.AdressId);
            
            values.District = command.District;
            values.City = command.City;
            values.UserId = command.UserId;
            values.Detail1 = values.Detail1;
            values.Detail2 = values.Detail2;
            values.Phone = values.Phone;
            values.Country = values.Country;
            values.ZipCode = values.ZipCode;
            values.Description = values.Description;
            values.Name = values.Name;
            values.Surname = values.Surname;

            await _repository.UpdateAsync(values);
        }
    }
}
