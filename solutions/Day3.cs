using System.IO;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public static class Day3
{
    public static void Part1()
    {
        var rawInput = File.ReadAllLines("input/day3.txt");
        var codeLength = rawInput.First().Count();
        var parsedInput = rawInput.Select(x => Convert.ToInt32(x, 2)).ToList(); 

        var powerConsumption = CalculatePowerConsumption(parsedInput, codeLength);
        System.Console.WriteLine($"Part 1: {powerConsumption}");
    }

    public static void Part2()
    {
        var rawInput = File.ReadAllLines("input/day3.txt");
        var codeLength = rawInput.First().Count();
        var parsedInput = rawInput.Select(x => Convert.ToInt32(x, 2)).ToList(); 

        var oxygenGeneratorRating = CalculateGasRating(parsedInput, codeLength, (int zeros, int ones) => zeros > ones);
        var co2ScrubberRating = CalculateGasRating(parsedInput, codeLength, (int zeros, int ones) => zeros <= ones);

        System.Console.WriteLine($"Part 2: {oxygenGeneratorRating * co2ScrubberRating}");
    }

    private static int CalculateGasRating(IEnumerable<int> diagnosticCodes, int codeLength, Func<int, int, bool> Compare)
    {
        if (diagnosticCodes.Count() == 1)
            return diagnosticCodes.First();

        var lookup = diagnosticCodes.ToLookup(code => (code >> codeLength - 1) & 1, code => code);

        return Compare(lookup[0].Count(), lookup[1].Count())
            ? CalculateGasRating(lookup[0], codeLength - 1, Compare)
            : CalculateGasRating(lookup[1], codeLength - 1, Compare);
    }

    private static int CalculatePowerConsumption(List<int> diagnosticCodes, int codeLength)
    {
        var buckets = new int[codeLength];

        foreach (var code in diagnosticCodes)
        {
            for (int i = 0; i < codeLength; i++)
            {
                int value = (code >> i) & 1;
                buckets[i] += value;
            }
        }

        var limit = diagnosticCodes.Count() / 2;
        var gammaRate = 0;

        for (int i = 0; i < codeLength; i++)
        {
            if (buckets[i] > limit)
                gammaRate += 1 << i; 
        }

        var mask = Convert.ToInt32(new String('1', codeLength), 2);
        var epsilonRate = ~gammaRate & mask;

        return gammaRate * epsilonRate;
    }
}