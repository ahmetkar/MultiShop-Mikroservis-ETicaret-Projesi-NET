namespace MultiShop.Cargo.WebApi.Events
{
    public class CargoOperationCreated
    {
        public int CargoOperationId { get; set; }
        public DateTime OperationDate { get; set; }

        public string CargoDetailId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
