using MultiShop.Catalog.DTOs.CategoryDTOs;

namespace MultiShop.Catalog.DTOs.ProductDTOs
{
    public class ResultProductsWithCategoryDto
    {
       
        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductDescription { get; set; }
        public ResultCategoryDto Category { get; set; }
    }
}
