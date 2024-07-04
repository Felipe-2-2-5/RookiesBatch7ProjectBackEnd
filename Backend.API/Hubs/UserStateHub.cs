using Backend.Application.IHubs;
using Microsoft.AspNetCore.SignalR;

namespace Backend.API.Hubs
{
    public class UserStateHub : Hub, IUserStateHub
    {
        public async Task NotifyUserDisabled(int userId)
        {
            await Clients.User(userId).SendAsync("UserDisabled");
        }
    }
}
