using System.ComponentModel.DataAnnotations;

namespace MultiShop.DtoLayer.OrderDtos.OrderAddressDtos
{
    public class UpdateOrderAddressDto
    {
        public int AdressId { get; set; }
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad zorunludur.")]
        [Display(Name = "Ad")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        public string Country { get; set; } = "Türkiye";

        [Required(ErrorMessage = "İlçe zorunludur.")]
        [Display(Name = "İlçe")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şehir zorunludur.")]
        [Display(Name = "Şehir")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres detayı zorunludur.")]
        [Display(Name = "Adres Detayı")]
        public string Detail1 { get; set; } = string.Empty;

        public string? Detail2 { get; set; }
        public string? Description { get; set; }
        public string? ZipCode { get; set; }

        public bool IsBillingOrShipping { get; set; }
    }
}