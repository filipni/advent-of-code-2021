using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

static class Day6
{
    private static IEnumerable<int> Input => File.ReadAllText("input/day6.txt")
                                                 .Split(',')
                                                 .Select(int.Parse);

    public static void Part1()
    {
        var numberOfFish = SimulateFish(Input, 80);
        Console.WriteLine($"Part 1: {numberOfFish}"); 
    }

    public static void Part2()
    {
        var numberOfFish = SimulateFish(Input, 256);
        Console.WriteLine($"Part 1: {numberOfFish}"); 
    }

    private static long SimulateFish(IEnumerable<int> ages, int days)
    {
        Dictionary<int, long> ageCount = new()
        {
            [-1] = 0,
            [0] = 0,
            [1] = 0,
            [2] = 0,
            [3] = 0,
            [4] = 0,
            [5] = 0,
            [6] = 0,
            [7] = 0,
            [8] = 0
        };

        foreach (var age in ages)
            ageCount[age] += 1;

        while (days > 0)
        {
            Dictionary<int, long> updatedAgeCount = new();
            for (int i = 0; i <= 8; i++)
                updatedAgeCount[i - 1] = ageCount[i];

            updatedAgeCount[8] = updatedAgeCount[-1];
            updatedAgeCount[6] = updatedAgeCount[6] + updatedAgeCount[-1];
            updatedAgeCount[-1] = 0;

            ageCount = updatedAgeCount;
            days--;
        }

        return ageCount.Sum(kvp => kvp.Value);
    }
}