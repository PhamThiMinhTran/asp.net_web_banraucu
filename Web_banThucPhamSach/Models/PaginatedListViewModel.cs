using Microsoft.EntityFrameworkCore;

namespace Web_banThucPhamSach.Models
{
    public class PaginatedListViewModel<T> : List<T>
    {
        public int TotalCount { get; set; } //tổng số dòng của câu truy vấn
        public int PageSize { get; set; } //Số lượng dòng trên mỗi trang
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int PageIndex { get; set; }
        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;
        public int PrevPageIndex => Math.Max(1, PageIndex - 1);
        public int NextPageIndex => Math.Min(PageIndex + 1, TotalPages);
        public string PreviouDisabledAttr => HasPrevious ? string.Empty : "disabled";
        public string NextDisabledAttr => HasNext ? string.Empty : "disabled";
        public int ItemIndexFrom => (PageIndex - 1) * PageSize + 1;
        public int ItemIndexTo => Math.Min(TotalCount, PageIndex * PageSize);
        public PaginatedListViewModel(List<T> items, int totalCount, int pageIndex, int pageSize = 10)
        {
            AddRange(items);
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public static async Task<PaginatedListViewModel<T>> FromQueryAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            pageIndex = Math.Max(pageIndex, 1);
            pageSize = Math.Max(pageSize, 1);
            return new(await source.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync(), count, pageIndex, pageSize);

        }
    }
}
