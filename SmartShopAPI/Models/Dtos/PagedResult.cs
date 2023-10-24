using Microsoft.AspNetCore.Http.Features;

namespace SmartShopAPI.Models.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalPages { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (pageNumber < 1 || pageNumber > TotalPages)
            {
                Items = new List<T>();
                ItemsFrom = 0;
                ItemsTo = 0;
            }
            else
            {
                Items = items;
                ItemsFrom = pageSize * (pageNumber - 1) + 1;
                ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalCount);
            }
        }
    }
}
