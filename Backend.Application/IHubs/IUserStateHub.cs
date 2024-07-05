namespace Backend.Application.IHubs
{
    public interface IUserStateHub
    {
        Task NotifyUserDisabled(string userId);
    }
}