namespace Talabat.APIs.Helpers
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(int pageNumber, int pageSize, int pageCount, IReadOnlyList<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = pageCount;
            Data = data;

        }
        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }


        public IReadOnlyList<T> Data { get; set; }
    }
}
