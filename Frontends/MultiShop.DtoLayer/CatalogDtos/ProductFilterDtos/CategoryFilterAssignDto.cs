namespace MultiShop.DtoLayer.CatalogDtos.ProductFilterDtos
{
    public class CategoryFilterAssignDto
    {
        public string CategoryId { get; set; }
        public List<string> FilterIds { get; set; } = new List<string>();
    }
}
