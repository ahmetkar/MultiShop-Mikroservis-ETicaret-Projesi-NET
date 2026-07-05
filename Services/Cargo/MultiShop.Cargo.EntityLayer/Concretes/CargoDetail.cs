using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.EntityLayer.Concretes
{
    public class CargoDetail
    {
        public CargoDetail() { }

        public int CargoDetailId { get; set; }
        

        public CargoCustomer Customer { get; set; }
        public int CustomerId { get; set; }
        public string Barcode { get; set; }

        public CargoCompany CargoCompany { get; set; }
        public int CargoCompanyId { get; set; }

        public CargoOperation CargoOperation { get; set; }


    }
}
