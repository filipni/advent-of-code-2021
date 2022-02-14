using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Day13
{
    private enum FoldDirection { Left, Up }

    private static IEnumerable<(int, int)> Dots => ParseDots(File.ReadAllLines("input/day13.txt"));
    private static IEnumerable<(FoldDirection, int)> Folds => ParseFolds(File.ReadAllLines("input/day13.txt"));

    private static IEnumerable<(int, int)> ParseDots(string[] lines)
    {
        var regex = new Regex(@"(\d+),(\d+)");
        List<(int, int)> dots = new();

        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                dots.Add((x, y));
            }
        }

        return dots;
    }

    private static IEnumerable<(FoldDirection, int)> ParseFolds(string[] lines)
    {
        var regex = new Regex(@"fold along ([yx]){1}=(\d+)");
        List<(FoldDirection, int)> folds = new();

        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                var direction = match.Groups[1].Value == "y" ? FoldDirection.Up : FoldDirection.Left;
                var value = int.Parse(match.Groups[2].Value);
                folds.Add((direction, value));
            }
        }

        return folds;
    }
    
    private static IEnumerable<(int, int)> Fold(IEnumerable<(int x, int y)> dots,
                                                IEnumerable<(FoldDirection direction, int line)> folds,
                                                int numberOfFolds = 1)
    {
        var selectedFolds = folds.Take(numberOfFolds);
        var width = folds.Where(fold => fold.direction == FoldDirection.Left).Max(fold => fold.line) * 2 + 1;
        var height = folds.Where(fold => fold.direction == FoldDirection.Up).Max(fold => fold.line) * 2 + 1;

        foreach (var (direction, line) in selectedFolds)
        {
            if (direction == FoldDirection.Up)
            {
                var unchangedDots = dots.Where(dot => dot.y < line);
                var foldedDots = dots.Where(dot => dot.y > line)
                                     .Select(dot => (dot.x, height - dot.y - 1))
                                     .ToList();
                dots = unchangedDots.Union(foldedDots);
                height = (height - 1) / 2;
            }
            else if (direction == FoldDirection.Left)
            {
                var unchangedDots = dots.Where(dot => dot.x < line);
                var foldedDots = dots.Where(dot => dot.x > line)
                                     .Select(dot => (width - dot.x - 1, dot.y))
                                     .ToList();
                dots = unchangedDots.Union(foldedDots);
                width = (width - 1) / 2;
            }
        }

        return dots;
    }

    public static void Part1()
    {
        var result = Fold(Dots, Folds);
        Console.WriteLine($"Part 1: {result.Count()}");
    }

    public static void Part2()
    {
        var folds = Folds.ToList();
        var dots = Fold(Dots, folds, folds.Count());
        Console.WriteLine("Part 2:");
        PrintDots(dots);
    }

    private static void PrintDots(IEnumerable<(int, int)> dots)
    {
        var width = dots.Max(dot => dot.Item1) + 1; 
        var height = dots.Max(dot => dot.Item2) + 1;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var symbol = dots.Contains((j, i)) ? '⬜' : '⬛';
                System.Console.Write(symbol);
            }
            System.Console.WriteLine();
        }
    }


}