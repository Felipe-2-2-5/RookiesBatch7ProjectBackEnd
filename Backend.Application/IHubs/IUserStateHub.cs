
namespace Backend.Application.IHubs
{
    public interface IUserStateHub
    {
        Task NotifyUserDisabled(int userId);
    }
}