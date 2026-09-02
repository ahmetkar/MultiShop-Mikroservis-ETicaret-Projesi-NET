namespace MultiShop.Catalog.DTOs.CategoryDTOs
{
    public class ResultCategoryDto
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public List<string> SelectedFilterIds { get; set; } = new List<string>();
    }
}
