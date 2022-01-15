using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

static class Day6
{
    private static IEnumerable<int> Input => File.ReadAllText("input/day6.txt")
                                                 .Split(',')
                                                 .Select(int.Parse);

    public static void Part1()
    {
        var numberOfFish = SimulateFish(Input, 80).Count();
        Console.WriteLine($"Part 1: {numberOfFish}"); 
    }

    public static void Part2()
    {
        var numberOfFish = SimulateFish(Input, 256).Count();
        Console.WriteLine($"Part 1: {numberOfFish}"); 
    }

    private static IEnumerable<int> SimulateFish(IEnumerable<int> ages, int days)
    {
        while (days > 0)
        {
            ConcurrentBag<int> updatedAges = new();

            Parallel.ForEach(ages, age =>
            {
                if (age == 0)
                {
                    updatedAges.Add(6);
                    updatedAges.Add(8);
                }
                else
                {
                    updatedAges.Add(age - 1);
                }
            });

            ages = updatedAges;
            days--;
        }

        return ages;
    }
}