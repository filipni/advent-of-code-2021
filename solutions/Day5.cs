using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Day5
{
    public record Point(int X, int Y);

    private static IEnumerable<(Point, Point)> GetInput()
    {
        var rawInput = File.ReadAllLines("input/day5.txt");
        List<(Point, Point)> parsedInput = new();

        var regex = new Regex(@"^(\d+),(\d+) -> (\d+),(\d+)$");

        foreach (var line in rawInput)
        {
           var match = regex.Match(line); 
           if (match.Success)
           {
               var x1 = int.Parse(match.Groups[1].Value);
               var y1 = int.Parse(match.Groups[2].Value);
               Point point1 = new(x1, y1);

               var x2 = int.Parse(match.Groups[3].Value);
               var y2 = int.Parse(match.Groups[4].Value);
               Point point2 = new(x2, y2);

               parsedInput.Add((point1, point2));
           }
        }

        return parsedInput;
    }

    public static int GetNumberOfOverlappingPoints(IEnumerable<(Point, Point)> input, bool includeDiagonalLines = false)
    {
        Dictionary<Point, int> coveredPoints = new();

        foreach (var (start, end) in input)
        {
            var points = GetPointsInLine(start, end, includeDiagonalLines); 
            foreach (var point in points)
                coveredPoints[point] = coveredPoints.GetValueOrDefault(point) + 1;
        }

        return coveredPoints.Where(kvp => kvp.Value >= 2).Count();
    }

    private static IEnumerable<Point> GetPointsInLine(Point start, Point end, bool includeDiagonalLines = false)
    {
        if (start.X == end.X)
            return GetPointsInVerticalLine(start, end);
        else if (start.Y == end.Y)
            return GetPointsInHorizontalLine(start, end);
        else if (includeDiagonalLines) 
            return GetPointsInDiagonalLine(start, end);
        else
            return new List<Point>();
    }

    private static IEnumerable<Point> GetPointsInVerticalLine(Point start, Point end)
    {
        var count = Math.Abs(start.Y - end.Y) + 1;
        var xRange = Enumerable.Repeat(start.X, count);

        var yStart = Math.Min(start.Y, end.Y);
        var yRange = Enumerable.Range(yStart, count);

        return xRange.Zip(yRange, (x, y) => new Point(x,y));
    }

    private static IEnumerable<Point> GetPointsInHorizontalLine(Point start, Point end)
    {
        var count = Math.Abs(start.X - end.X) + 1;
        var yRange = Enumerable.Repeat(start.Y, count);

        var xStart = Math.Min(start.X, end.X);
        var xRange = Enumerable.Range(xStart, count);

        return xRange.Zip(yRange, (x, y) => new Point(x,y));
    }

    private static IEnumerable<Point> GetPointsInDiagonalLine(Point start, Point end)
    {   // Coefficients for linear equation
        var k = (start.Y - end.Y) / (start.X - end.X);
        var m = -(k*start.X - start.Y);

        var xStart = Math.Min(start.X, end.X);
        var count = Math.Abs(start.X - end.X) + 1;
        var xRange = Enumerable.Range(xStart, count);

        return xRange.Select(x => new Point(x, k*x + m));
    }

    public static void Part1()
    {
        var numberOfOverlappingPoints = GetNumberOfOverlappingPoints(GetInput());
        Console.WriteLine($"Part 1: {numberOfOverlappingPoints}");
    }

    public static void Part2()
    {
        var numberOfOverlappingPoints = GetNumberOfOverlappingPoints(GetInput(), true);
        Console.WriteLine($"Part 2: {numberOfOverlappingPoints}");
    }
}