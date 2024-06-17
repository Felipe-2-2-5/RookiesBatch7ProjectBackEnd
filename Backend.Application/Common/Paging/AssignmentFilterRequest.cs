using Backend.Application.Common.Converter;
using Newtonsoft.Json;

namespace Backend.Application.Common.Paging;

public class AssignmentFilterRequest : BaseFilterRequest
{
    public string? State { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}