namespace Backend.Application.Common.Paging
{
    public class ReturnRequestFilterRequest : BaseFilterRequest
    {
        public string? State { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}
