using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TextTemplating;

namespace SignalRSample.Hubs
{
    public class UserHub : Hub
    {
        public static int TotalViews { get; set; } = 0;
        public static int TotalUsers { get; set; } = 0;

        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<string> NewWindowLoaded(string name)
        {
            TotalViews++;

            // Send an update to all the connected clients that some data has been updated.
            await Clients.All.SendAsync("updateTotalViews", TotalViews);

            return $"Total views are {TotalViews} and sent from {name}";
        }
    }
}
