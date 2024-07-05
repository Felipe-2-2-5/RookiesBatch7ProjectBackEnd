using Backend.Application.IHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Backend.API.Hubs
{
    [Authorize]
    public class UserStateHub : Hub, IUserStateHub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("UserId")?.Value;
            if (userId != null)
            {
                _userConnections[userId] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("UserId")?.Value;
            if (userId != null)
            {
                _userConnections.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyUserDisabled(string userId)
        {
            if (_userConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("UserDisabled");
            }
        }
    }
}
