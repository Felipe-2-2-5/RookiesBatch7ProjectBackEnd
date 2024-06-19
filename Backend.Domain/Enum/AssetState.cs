namespace Backend.Domain.Enum
{
    public enum AssetState
    {
        Available = 0,
        NotAvailable = 1,
        WaitingForRecycling = 2,
        Recycled = 3,
        Assigned = 4
    }
}
