using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;

namespace SignalRSample.Hubs
{
    public class BasicChatHub : Hub
    {
        private readonly ApplicationDbContext applicationDb;

        public BasicChatHub(ApplicationDbContext applicationDb)
        {
            this.applicationDb = applicationDb;
        }

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
    }
}
