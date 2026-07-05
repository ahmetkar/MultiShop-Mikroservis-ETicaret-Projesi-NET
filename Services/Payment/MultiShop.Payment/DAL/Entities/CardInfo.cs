using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShop.Payment.DAL.Entities
{
    public class CardInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CardInfoId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string CardType { get; set; }
        public string CardBrand { get; set; }
        public string LastDateYear { get; set; }
        public string LastDateMonth { get; set; }
        public string LastFourNumber { get; set; }
        public string CardBankName { get; set; }
    }
}
