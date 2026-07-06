using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.Dtos.CargoOperation
{
    public class UpdateCargoOperationDto
    {
        public int CargoOperationId { get; set; }
        public int CargoDetailId { get; set; }
        public string Description { get; set; }

        public int OrderingId { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
