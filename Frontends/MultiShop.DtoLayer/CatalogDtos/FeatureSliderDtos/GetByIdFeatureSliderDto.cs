﻿namespace MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos
{
    public class GetByIdFeatureSliderDto
    {
        public string FeatureSliderID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
    }
}
