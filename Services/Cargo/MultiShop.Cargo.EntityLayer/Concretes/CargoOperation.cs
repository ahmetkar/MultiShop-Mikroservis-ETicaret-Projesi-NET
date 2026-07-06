using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.EntityLayer.Concretes
{
    public class CargoOperation
    {
        public int CargoOperationId { get; set; }

        public string Description { get; set; } 
        public DateTime OperationDate { get; set; }

        public int CargoDetailId { get; set; }
        public CargoDetail CargoDetail { get; set; }


        public int OrderingId { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
