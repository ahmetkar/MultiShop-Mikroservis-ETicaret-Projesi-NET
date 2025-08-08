using MultiShop.Order.Application.Features.CQRS.Commands.AdressCommands;
using MultiShop.Order.Application.Features.CQRS.Queries.AdressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AdressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AdressHandlers
{
    public class GetAdressByIdQueryHandler
    {
        private readonly IRepository<Adress> _repository;

        public GetAdressByIdQueryHandler(IRepository<Adress> repository)
        {
            _repository = repository;
        }

        public async Task<GetAdressByIdQueryResult> Handle(GetAdressByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
            return new GetAdressByIdQueryResult { 
                AdressId = values.AdressId,
                City = values.City,
                Detail1 = values.Detail1,
                Detail2 = values.Detail2,
                District = values.District,
                UserId = values.UserId,
                Phone = values.Phone,
                Country = values.Country,
                ZipCode = values.ZipCode,
                Description = values.Description,
                Name = values.Name,
                Surname = values.Surname
            };
        }
    }
}
