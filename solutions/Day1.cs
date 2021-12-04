using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day1
{
    private static List<int> Input => File.ReadAllLines("input/day1.txt")
                                          .Select(x => int.Parse(x))
                                          .ToList();

    public static void Part1()
    {
        int numberOfIncreases = CalculateNumberOfIncreases(Input);
        System.Console.WriteLine($"Part 1: {numberOfIncreases}");
    }

    public static void Part2()
    {
        int numberOfIncreases = CalculateNumberOfIncreases(Input, 3);
        System.Console.WriteLine($"Part 2: {numberOfIncreases}");
    }

    private static int CalculateNumberOfIncreases(List<int> measurements, int windowSize = 1)
    {
        if (measurements.Count <= windowSize)
            return 0;

        var previousValue = measurements.SumRange(0, windowSize);
        var numberOfIncreases = 0;

        for (int i = 1; i + windowSize - 1 < measurements.Count; i++)
        {
            var currentValue = measurements.SumRange(i, windowSize);

            if (currentValue > previousValue)
                numberOfIncreases++;   

            previousValue = currentValue;
        }

        return numberOfIncreases;
    }

    private static int SumRange(this List<int> numbers, int startIndex, int count)
    {
        int sum = 0;

        for (int i = startIndex; i < startIndex + count; i++)
            sum += numbers[i];

        return sum;
    }
}