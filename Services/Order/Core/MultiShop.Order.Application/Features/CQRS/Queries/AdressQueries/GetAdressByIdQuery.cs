﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Queries.AdressQueries
{
    public class GetAdressByIdQuery
    {
        public GetAdressByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
