using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class Day8
{
    private static List<List<string>> Inputs { get; }

    private static List<List<string>> Outputs { get; }

    private static readonly List<char> _segments = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

    private static readonly Dictionary<string, char> _segmentsToDigit = new()
    {
        ["abcefg"] = '0',
        ["cf"] = '1',
        ["acdeg"] = '2',
        ["acdfg"] = '3',
        ["bcdf"] = '4',
        ["abdfg"] = '5',
        ["abdefg"] = '6',
        ["acf"] = '7',
        ["abcdefg"] = '8',
        ["abcdfg"] = '9',
    }; 

    private static readonly Dictionary<int, List<char>> _signalCountToSegments = new()
    {
        [2] = new() { 'c', 'f' },
        [4] = new() { 'b', 'c', 'd', 'f' },
        [3] = new() { 'a', 'c', 'f' },
        [5] = new() { 'a', 'd', 'g' },
        [6] = new() { 'a', 'b', 'f', 'g' },
        [7] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }
    };

    static Day8()
    {
        List<List<string>> inputs = new();
        List<List<string>> outputs = new();

        var lines = File.ReadAllLines("input/day8.txt");

        foreach (var line in lines)
        {
            var regex = new Regex(@"(((\w+) ?)+) \| (((\w+) ?)+)");
            var match = regex.Match(line);

            if (!match.Success) continue;

            var input = match.Groups[3].Captures.Select(capture => capture.Value).ToList();
            inputs.Add(input);

            var output = match.Groups[6].Captures.Select(capture => capture.Value).ToList();
            outputs.Add(output);
        }

        Inputs = inputs;
        Outputs = outputs;
    }

    public static void Part1()
    {
        List<int> validLengths = new() { 2, 3, 4, 7 };
        var numberOfDigits = Outputs.SelectMany(output => output)
                                    .Count(signals => validLengths.Contains(signals.Length));
        Console.WriteLine($"Part 1: {numberOfDigits}");
    }

    public static void Part2()
    {
        var sumOfOutputs = 0;

        for (int i = 0; i < Inputs.Count(); i++)
        {
            var segmentCandidateSignals = _segments.ToDictionary(c => c, c => new HashSet<char>());

            foreach (var signals in Inputs[i])
            {
                var numberOfSignals = signals.Count();
                if (_signalCountToSegments.ContainsKey(numberOfSignals))
                {
                    var segments = _signalCountToSegments[numberOfSignals];

                    foreach (var segment in segments)
                    {
                        var candidateSignals = segmentCandidateSignals[segment]; 
                        if (candidateSignals.Any())
                            candidateSignals.IntersectWith(new HashSet<char>(signals));
                        else
                            candidateSignals.UnionWith(new HashSet<char>(signals));
                    }
                }
            }
 
            // Remove candidates until all segments are defined
            while (segmentCandidateSignals.Any(kvp => kvp.Value.Count() > 1))
            {
                foreach (var singleCandidateSet in segmentCandidateSignals.Values)
                {
                    if (singleCandidateSet.Count != 1) continue;

                    foreach (var candidateSignals in segmentCandidateSignals.Values)
                    {
                        if (singleCandidateSet != candidateSignals)
                            candidateSignals.Remove(singleCandidateSet.First());         
                    }
                }
            }

            var signalToSegment = segmentCandidateSignals.ToDictionary(kvp => kvp.Value.First(), kvp => kvp.Key);

            var translatedOutput = Outputs[i].Select(signals => ConvertSignalsToSegments(signalToSegment, signals));
            var outputDigits = translatedOutput.Select(segments => _segmentsToDigit[segments]).ToArray();
            var outputValue = int.Parse(new String(outputDigits));

            sumOfOutputs += outputValue;
        }

        Console.WriteLine($"Part 2: {sumOfOutputs}");
    } 

    private static string ConvertSignalsToSegments(Dictionary<char, char> signalToSegment, string signals)
    {
        var segments = signals.Select(signal => signalToSegment[signal])
                              .OrderBy(segment => segment)
                              .ToArray();
        return new String(segments);
    }
}