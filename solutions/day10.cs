using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day10
{
    private static string[] Input => File.ReadAllLines("input/day10.txt");

    private static List<char> _openingCharacters = new() { '(', '[', '{', '<' }; 

    private static Dictionary<char, char> _matchingOpeningCharacter = new()
    {
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{',
        ['>'] = '<'
    };

    private static Dictionary<char, int> _scoreTableForCorruptLines = new()
    {
        [')'] = 3,
        [']'] = 57,
        ['}'] = 1197,
        ['>'] = 25137
    };

    private static Dictionary<char, int> _scoreTableForIncompleteLines = new()
    {
        ['('] = 1,
        ['['] = 2,
        ['{'] = 3,
        ['<'] = 4
    };

    public static void Part1()
    {
        var errorScore = 0;

        foreach (var line in Input)
        {
            Stack<char> stack = new();
            var lineIsCorrupted = false;

            foreach (var c in line)
            {
                if (_openingCharacters.Contains(c))
                    stack.Push(c);
                else
                    lineIsCorrupted = stack.Pop() != _matchingOpeningCharacter[c];

                if (lineIsCorrupted)
                {
                    errorScore += _scoreTableForCorruptLines[c];
                    break;
                }
            }
        }

        Console.WriteLine($"Part 1: {errorScore}");
    }

    public static void Part2()
    {
        List<long> errorScores = new();

        foreach (var line in Input)
        {
            Stack<char> stack = new();
            var lineIsCorrupted = false;

            foreach (var c in line)
            {
                if (_openingCharacters.Contains(c))
                    stack.Push(c);
                else
                    lineIsCorrupted = stack.Pop() != _matchingOpeningCharacter[c];

                if (lineIsCorrupted)
                    break;
            }

            var lineErrorScore = 0L;
            if (!lineIsCorrupted)
            {
                while (stack.Any())
                    lineErrorScore = lineErrorScore * 5 + _scoreTableForIncompleteLines[stack.Pop()];
                errorScores.Add(lineErrorScore);
            }
        }

        errorScores.Sort();
        var middleIndex = errorScores.Count() / 2;

        Console.WriteLine($"Part 2: {errorScores[middleIndex]}");
    }
}