using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day1
{
    public static void Part1()
    {
        int numberOfIncreases = CalculateNumberOfIncreases(1);
        System.Console.WriteLine($"Part 1: {numberOfIncreases}");
    }

    public static void Part2()
    {
        int numberOfIncreases = CalculateNumberOfIncreases(3);
        System.Console.WriteLine($"Part 2: {numberOfIncreases}");
    }

    public static int CalculateNumberOfIncreases(int windowSize)
    {
        var input = File.ReadAllLines("input/day1.txt").Select(x => int.Parse(x)).ToList();
        if (input.Count <= windowSize)
            return 0;

        var previousValue = input.SumRange(0, windowSize);
        var numberOfIncreases = 0;

        for (int i = 1; i + windowSize - 1 < input.Count; i++)
        {
            var currentValue = input.SumRange(i, windowSize);

            if (currentValue > previousValue)
                numberOfIncreases++;   

            previousValue = currentValue;
        }

        return numberOfIncreases;
    }

    public static int SumRange(this List<int> numbers, int startIndex, int count)
    {
        int sum = 0;

        for (int i = startIndex; i < startIndex + count; i++)
            sum += numbers[i];

        return sum;
    }
}