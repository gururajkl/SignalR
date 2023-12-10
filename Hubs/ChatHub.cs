using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using System.Security.Claims;

namespace SignalRSample.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext applicationDb;

        public ChatHub(ApplicationDbContext applicationDb)
        {
            this.applicationDb = applicationDb;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var userName = applicationDb.Users.FirstOrDefault(u => u.Id == userId)!.UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("receiveUserConnected", userId, userName);

                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);

            if (HubConnections.HasUserConnection(userId!, Context.ConnectionId))
            {
                var userConnections = HubConnections.Users[userId!];
                userConnections.Remove(Context.ConnectionId);

                HubConnections.Users.Remove(userId!);
                if (userConnections.Any())
                {
                    HubConnections.Users.Add(userId!, userConnections);
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var userName = applicationDb.Users.FirstOrDefault(u => u.Id == userId)!.UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("receiveUserDisconnected", userId, userName);

                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAddRoomMessage(int maxRoom, int roomId, string roomName)
        {
            var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = applicationDb.Users.FirstOrDefault(u => u.Id == userId)!.UserName;

            await Clients.All.SendAsync("receiveAddRoomMessage", maxRoom, roomId, roomName, userId, userName);
        }
        
        public async Task SendDeleteRoomMessage(int deleted, int selected, string roomName)
        {
            var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = applicationDb.Users.FirstOrDefault(u => u.Id == userId)!.UserName;

            await Clients.All.SendAsync("receiveDeleteRoomMessage", deleted, selected, roomName, userName);
        }

        /*
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("messageReceived", user, message);
        }

        [Authorize]
        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {
            var userId = applicationDb.Users.FirstOrDefault(u => u.Email!.ToLower() == receiver.ToLower())!.Id;

            if (userId != null)
            {
                await Clients.User(userId).SendAsync("messageReceived", sender, message);
            }
        }
        */
    }
}
