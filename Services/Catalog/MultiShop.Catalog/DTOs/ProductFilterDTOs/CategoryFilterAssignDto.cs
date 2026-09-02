namespace MultiShop.Catalog.DTOs.ProductFilterDTOs
{
    public class CategoryFilterAssignDto
    {
        public string CategoryId { get; set; }
        public List<string> FilterIds { get; set; } = new List<string>();
    }
}
