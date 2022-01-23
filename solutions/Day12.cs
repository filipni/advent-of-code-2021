using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day12
{
    private static Dictionary<string, List<string>> _neighbours =
        File.ReadAllLines("input/day12.txt")
            .Select(ParseLine)
            .SelectMany(tuples => tuples)
            .GroupBy(x => x.Item1, x => x.Item2)
            .ToDictionary(x => x.Key, x => x.ToList());

    private static List<(string, string)> ParseLine(string line)
    {
        var split = line.Split('-');
        return new List<(string, string)>
        {
            (split[0], split[1]),
            (split[1], split[0])
        };
    }

    public static void Part1()
    {
        var foundPaths = FindPaths(new HashSet<string>(), new List<string>(), "start");
        System.Console.WriteLine($"Part 1: {foundPaths.Count()}");
    }

    private static IEnumerable<IEnumerable<string>> FindPaths(IEnumerable<string> visitedSmallCaves,
                                                              IEnumerable<string> path,
                                                              string currentCave)
    {
        path = path.Append(currentCave);
        if (currentCave == "end")
            return new List<IEnumerable<string>> { path };

        visitedSmallCaves = currentCave.Any(char.IsLower)
            ? visitedSmallCaves.Append(currentCave)
            : visitedSmallCaves;

        IEnumerable<IEnumerable<string>> paths = new List<IEnumerable<string>>();
        var neighbouringCaves = _neighbours[currentCave];

        foreach (var cave in neighbouringCaves)
        {
            if (visitedSmallCaves.Contains(cave)) continue;

            var foundPaths = FindPaths(visitedSmallCaves, path, cave);
            paths = paths.Concat(foundPaths);
        }

        return paths;
    }
}