using MultiShop.DtoLayer.CatalogDtos.FilterDtos;

namespace MultiShop.WebUI.Helpers
{
    public static class ProductStockHelper
    {
        public static bool IsProductOutOfStock(
            List<string>? filterIds,
            Dictionary<string, int>? filterStocks,
            List<ResultFilterDto>? allFilters)
        {
            if (filterIds == null || filterIds.Count == 0)
            {
                return false;
            }

            if (filterStocks == null || filterStocks.Count == 0)
            {
                return true;
            }

            if (allFilters == null || allFilters.Count == 0)
            {
                return filterStocks.Values.All(v => v <= 0);
            }

            var prodFilters = allFilters.Where(f => filterIds.Contains(f.FilterId)).ToList();
            if (prodFilters.Count == 0)
            {
                return filterStocks.Values.All(v => v <= 0);
            }

            var grouped = prodFilters.GroupBy(f => f.FilterTitle);
            foreach (var group in grouped)
            {
                // Each required filter title must have at least one variant option with stock >= 1
                bool hasAvailableOptionInGroup = group.Any(f => filterStocks.TryGetValue(f.FilterId, out int s) && s > 0);
                if (!hasAvailableOptionInGroup)
                {
                    return true;
                }
            }

            return false;
        }
    }
}