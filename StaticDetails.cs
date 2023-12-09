namespace SignalRSample;

public static class StaticDetails
{
    static StaticDetails()
    {
        deathlyHallowRace = new Dictionary<string, int>();
        deathlyHallowRace.Add(wand, 0);
        deathlyHallowRace.Add(stone, 0);
        deathlyHallowRace.Add(cloak, 0);
    }

    public const string wand = "wand";
    public const string stone = "stone";
    public const string cloak = "cloak";

    public static Dictionary<string, int> deathlyHallowRace;
}
