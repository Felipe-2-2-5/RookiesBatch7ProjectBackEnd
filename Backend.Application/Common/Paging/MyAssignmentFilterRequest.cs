namespace Backend.Application.Common.Paging;

public class MyAssignmentFilterRequest
{
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int UserId { get; set; } 
}