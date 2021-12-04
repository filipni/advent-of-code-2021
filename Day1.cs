using System.IO;
using System.Linq;

public class Day1
{
    public void Part1()
    {
        var input = File.ReadAllLines("input/day1.txt").Select(x => int.Parse(x)).ToList();
        if (input.Count <= 1)
            return;

        var previousValue = input[0];
        var numberOfIncreases = 0;

        for (int i = 1; i < input.Count; i++)
        {
            var currentValue = input[i];

            if (currentValue > previousValue)
                numberOfIncreases++;   

            previousValue = currentValue;
        }

        System.Console.WriteLine($"Part 1: {numberOfIncreases}");
    }

    public void Part2()
    {
        var input = File.ReadAllLines("input/day1.txt").Select(x => int.Parse(x)).ToList();
        if (input.Count <= 3)
            return;

        var previousValue = input[0] + input[1] + input[2];
        var numberOfIncreases = 0;

        for (int i = 1; i + 2 < input.Count; i++)
        {
            var currentValue = input[i] + input[i + 1] + input[i + 2];

            if (currentValue > previousValue)
                numberOfIncreases++;   

            previousValue = currentValue;
        }

        System.Console.WriteLine($"Part 2: {numberOfIncreases}");
    }
}