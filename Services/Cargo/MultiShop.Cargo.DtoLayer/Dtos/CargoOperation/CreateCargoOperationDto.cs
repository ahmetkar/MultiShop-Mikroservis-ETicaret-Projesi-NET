using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.Dtos.CargoOperation
{
    public class CreateCargoOperationDto
    {
      
        public int CargoDetailId { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
