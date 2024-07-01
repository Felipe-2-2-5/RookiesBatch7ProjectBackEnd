namespace Backend.Application.Common.Paging
{
    public class ReturnRequestFilterRequest
    {
        public string State { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
