using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.Dtos.CargoDetail
{
    public class UpdateCargoDetailDto
    {
        public int CargoDetailId { get; set; }
        public int CustomerId { get; set; }
        public string Barcode { get; set; }
        public int CargoCompanyId { get; set; }
    }
}
