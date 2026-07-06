using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.EntityLayer.Concretes
{
    public class CargoCustomer
    {
        public int CargoCustomerId { get; set; }

        public string? UserCustomerId { get; set; }

        public List<CargoDetail> CargoDetails { get; set; }
    }
}
    