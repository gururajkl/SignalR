using Microsoft.AspNetCore.SignalR;

namespace SignalRSample;

public class HouseGroupHub : Hub
{
    public static List<string> GroupsJoined { get; set; } = new List<string>();

    public async Task JoinHouse(string houseName)
    {
        if (!GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
        {
            GroupsJoined.Add(Context.ConnectionId + ":" + houseName);
            await Groups.AddToGroupAsync(Context.ConnectionId, houseName);

            string houseLists = "";
            foreach (string item in GroupsJoined)
            {
                if (item.Contains(Context.ConnectionId))
                {
                    houseLists += item.Split(":")[1] + " ";
                }
            }

            await Clients.Caller.SendAsync("subscriptionStatus", houseLists, houseName.ToLower(), true);
            await Clients.Others.SendAsync("subscriptionStatusToast", houseName);
        }
    }

    public async Task LeaveHouse(string houseName)
    {
        if (GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
        {
            GroupsJoined.Remove(Context.ConnectionId + ":" + houseName);

            string houseLists = "";
            foreach (string item in GroupsJoined)
            {
                if (item.Contains(Context.ConnectionId))
                {
                    houseLists += item.Split(":")[1] + " ";
                }
            }
            await Clients.Caller.SendAsync("subscriptionStatus", houseLists, houseName.ToLower(), false);
            await Clients.Others.SendAsync("unSubscriptionStatusToast", houseName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);
        }
    }

    public async Task TriggerNofitication(string houseName)
    {
        await Clients.Groups(houseName).SendAsync("triggerNotification", houseName);
    }
}
