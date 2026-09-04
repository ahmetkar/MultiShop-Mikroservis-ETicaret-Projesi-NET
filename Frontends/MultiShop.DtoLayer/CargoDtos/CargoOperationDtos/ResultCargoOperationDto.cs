using System;

namespace MultiShop.DtoLayer.CargoDtos.CargoOperationDtos
{
    public class ResultCargoOperationDto
    {
        public int CargoOperationId { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
        public int CargoDetailId { get; set; }
        public int OrderingId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
