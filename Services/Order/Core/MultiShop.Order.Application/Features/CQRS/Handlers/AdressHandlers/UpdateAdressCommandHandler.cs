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
            if (values != null)
            {
                values.District = command.District ?? values.District ?? "";
                values.City = command.City ?? values.City ?? "";
                values.UserId = command.UserId ?? values.UserId ?? "";
                values.Detail1 = command.Detail1 ?? values.Detail1 ?? "";
                values.Detail2 = command.Detail2 ?? values.Detail2 ?? "";
                values.Phone = command.Phone ?? values.Phone ?? "";
                values.Country = string.IsNullOrWhiteSpace(command.Country) ? (values.Country ?? "Türkiye") : command.Country;
                values.ZipCode = command.ZipCode ?? values.ZipCode ?? "";
                values.Description = command.Description ?? values.Description ?? "";
                values.Name = command.Name ?? values.Name ?? "";
                values.Surname = command.Surname ?? values.Surname ?? "";
                values.Email = command.Email ?? values.Email ?? "";
                values.IsBillingOrShipping = command.IsBillingOrShipping;

                await _repository.UpdateAsync(values);
            }
        }
    }
}
