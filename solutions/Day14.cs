using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Day14
{
    private static Dictionary<string, string> InsertionRules => ParseRules(File.ReadAllText("input/day14.txt"));
    private static string Template => File.ReadAllLines("input/day14.txt").First();

    private static Dictionary<string, string> ParseRules(string text)
        => new Regex(@"(\w\w) -> (\w)")
                .Matches(text)
                .ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value);

    public static void Part1()
    {
        var elementCount = ExtendPolymer(Template, InsertionRules);
        var result = elementCount.Max(kvp => kvp.Value) - elementCount.Min(kvp => kvp.Value);
        Console.WriteLine($"Part 1: {result}");
    }

    public static void Part2()
    {
        var elementCount = ExtendPolymer(Template, InsertionRules, 40);
        var result = elementCount.Max(kvp => kvp.Value) - elementCount.Min(kvp => kvp.Value);
        Console.WriteLine($"Part 2: {result}");
    }

    private static Dictionary<char, long> ExtendPolymer(string template,
                                                        Dictionary<string, string> rules,
                                                        int steps = 10)
    {
        // Create initial state from template
        Dictionary<string, long> elementPairs = new();
        Dictionary<char, long> elementCount = new();

        for (int i = 0; i < template.Length - 1; i++)
        {
            var pair = template[i..(i + 2)];
            elementPairs[pair] = elementPairs.GetValueOrDefault(pair) + 1;
            elementCount[pair[0]] = elementCount.GetValueOrDefault(pair[0]) + 1;
        }

        elementCount[template.Last()] = elementCount.GetValueOrDefault(template.Last()) + 1;

        // Extend polymer from initial state
        while (steps > 0)
        {
            Dictionary<string, long> newElementPairs = new();

            foreach (var kvp in elementPairs)
            {
                var pair = kvp.Key;
                var newElement = rules[pair].First();

                elementCount[newElement] = elementCount.GetValueOrDefault(newElement) + kvp.Value;

                var newPair1 = $"{pair[0]}{newElement}";
                var newPair2 = $"{newElement}{pair[1]}";

                newElementPairs[newPair1] = newElementPairs.GetValueOrDefault(newPair1) + kvp.Value;
                newElementPairs[newPair2] = newElementPairs.GetValueOrDefault(newPair2) + kvp.Value;
            }

            elementPairs = newElementPairs;
            steps--;
        }

        return elementCount;
    }
}