using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day7
{
    private static IEnumerable<int> Input => File.ReadAllText("input/day7.txt")
                                                 .Split(',')
                                                 .Select(int.Parse);

    public static void Part1()
    {
        var totalCost = AlignCrabs(Input, CalculateFuelCostHumanWay);
        Console.WriteLine($"Part 1: {totalCost}");
    }

    public static void Part2()
    {
        var totalCost = AlignCrabs(Input, CalculateFuelCostCrabWay);
        Console.WriteLine($"Part 2: {totalCost}");
    }

    private static int AlignCrabs(IEnumerable<int> positions, Func<IEnumerable<int>, int, int> calculateFuelCost)
    {
        var minPosition = positions.Min();
        var maxPosition = positions.Max();
        var candidateDestinations = Enumerable.Range(minPosition, maxPosition - minPosition);

        var minFuelCost = int.MaxValue;

        foreach (var destination in candidateDestinations)
        {
            var fuelCost = calculateFuelCost(positions, destination);
            if (fuelCost < minFuelCost) minFuelCost = fuelCost;
        }

        return minFuelCost;
    }

    private static int CalculateFuelCostHumanWay(IEnumerable<int> positions, int destination)
        => positions.Sum(position => Math.Abs(position - destination));

    private static int CalculateFuelCostCrabWay(IEnumerable<int> positions, int destination)
        => positions.Sum(position => AddRange(1, Math.Abs(position - destination)));

    private static int AddRange(int start, int count)
        => Enumerable.Range(start, count).Sum();

}