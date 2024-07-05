namespace Backend.Application.Common.Paging
{
    public class AssetFilterRequest : BaseFilterRequest
    {
        public string State { get; set; }
        public string Category { get; set; }
    }
}
