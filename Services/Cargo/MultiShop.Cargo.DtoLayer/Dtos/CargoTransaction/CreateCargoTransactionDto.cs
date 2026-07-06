using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.Dtos.CargoTransaction
{
    public class CreateCargoTransactionDto
    {
        public string UserId { get; set; }
        public int CargoCompanyId { get; set; }

        public string Barcode { get; set; }


    }
}
