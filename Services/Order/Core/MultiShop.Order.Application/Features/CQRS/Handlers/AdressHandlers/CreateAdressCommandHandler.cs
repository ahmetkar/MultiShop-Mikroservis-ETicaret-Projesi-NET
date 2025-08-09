using MultiShop.Order.Application.Features.CQRS.Commands.AdressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AdressHandlers
{
    public class CreateAdressCommandHandler
    {
        private readonly IRepository<Adress> _repository;

        public CreateAdressCommandHandler(IRepository<Adress> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateAdressCommand values)
        {
            var adress = new Adress()
            {
                City = values.City,
                Detail1 = values.Detail1,
                Detail2 = values.Detail2,
                District = values.District,
                UserId = values.UserId,
                Email = values.Email,
                Phone = values.Phone,
                Country = values.Country,
                ZipCode = values.ZipCode,
                Description = values.Description,
                Name = values.Name,
                Surname = values.Surname,
                IsBillingOrShipping = values.IsBillingOrShipping

            };
            await _repository.CreateAsync(adress);

            return adress.AdressId;
        }

    }
}
