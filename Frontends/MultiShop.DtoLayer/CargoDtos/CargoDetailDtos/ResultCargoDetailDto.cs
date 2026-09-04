using System;

namespace MultiShop.DtoLayer.CargoDtos.CargoDetailDtos
{
    public class ResultCargoDetailDto
    {
        public int CargoDetailId { get; set; }
        public int CustomerId { get; set; }
        public string Barcode { get; set; }
        public int CargoCompanyId { get; set; }
        public string Status { get; set; } = "Kargoya Verildi";
        public bool IsDelivered { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
