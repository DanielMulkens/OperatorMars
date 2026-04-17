using System.Collections.Generic;

public static class CaveManager
{
    private static List<SimpleCavePortal> caves = new List<SimpleCavePortal>();

    public static void Register(SimpleCavePortal cave)
    {
        if (!caves.Contains(cave))
            caves.Add(cave);
    }

    public static void Unregister(SimpleCavePortal cave)
    {
        if (caves.Contains(cave))
            caves.Remove(cave);
    }

    public static List<SimpleCavePortal> GetCaves()
    {
        return caves;
    }
}