﻿namespace MultiShop.DtoLayer.CatalogDtos.ContactDTOs
{
    public class ResultContactDto
    {
        public string ContactId { get; set; }

        public string NameSurname { get; set; }

        public string Email { get; set; }
        public string Subject { get; set; }

        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}
