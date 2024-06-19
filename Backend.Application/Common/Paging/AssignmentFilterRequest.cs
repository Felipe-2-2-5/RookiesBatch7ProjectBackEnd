namespace Backend.Application.Common.Paging;

public class AssignmentFilterRequest : BaseFilterRequest
{
    public string? State { get; set; }
    
    public DateTime FromDate { get; set; }
    
    public DateTime ToDate { get; set; }
}