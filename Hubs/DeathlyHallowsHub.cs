using Microsoft.AspNetCore.SignalR;

namespace SignalRSample;

public class DeathlyHallowsHub : Hub
{
    public Dictionary<string, int> GetRaceStatus()
    {
        return StaticDetails.deathlyHallowRace;
    }
}
