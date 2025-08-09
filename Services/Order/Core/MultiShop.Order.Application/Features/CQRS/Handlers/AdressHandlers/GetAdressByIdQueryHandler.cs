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

        public async Task<List<GetAdressByIdQueryResult>> Handle(GetAdressByIdQuery query)
        {
            var values = await _repository.GetAllByFilterAsync(x=>x.UserId == query.Id);
            
            var list = new List<GetAdressByIdQueryResult>();
            foreach(var i in values)
            {
               var item =  new GetAdressByIdQueryResult
                {
                    AdressId = i.AdressId,
                    City = i.City,
                    Detail1 = i.Detail1,
                    Detail2 = i.Detail2,
                    District = i.District,
                    UserId = i.UserId,
                    Phone = i.Phone,
                    Country = i.Country,
                    ZipCode = i.ZipCode,
                    Description = i.Description,
                    Name = i.Name,
                    Surname = i.Surname,
                    Email = i.Email,
                    IsBillingOrShipping = i.IsBillingOrShipping
                };
                list.Add(item);
            }

            return list;
        }
    }
}
