using Backend.Domain.Enum;

namespace Backend.Application.Common.Paging
{
    public class UserFilterRequest
    {
        public Role? Role { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

}
