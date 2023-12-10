﻿using Microsoft.AspNetCore.Authorization;
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
