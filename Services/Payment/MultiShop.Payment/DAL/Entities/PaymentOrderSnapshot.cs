namespace MultiShop.Payment.DAL.Entities
{
    public class PaymentOrderSnapshot
    {

        public int PaymentOrderSnapshotId { get; set; }
        public string UserId { get; set; }

        public int OrderingId { get; set; }

        public int PaymentTotal { get; set; }

        public Guid CorrelationId {  get; set; }

        public bool IsSuccessful { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
