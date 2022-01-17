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

    public static int GetNumberOfOverlappingPoints(IEnumerable<(Point, Point)> input, bool ignoreDiagonalLines = true)
    {
        Dictionary<Point, int> coveredPoints = new();

        foreach (var (point1, point2) in input)
        {
            var straightLine = point1.X == point2.X || point1.Y == point2.Y; 
            var points = straightLine
                ? GetStraightLine(point1, point2)
                : GetDiagonalLine(point1, point2);

            foreach (var point in points)
                if (straightLine || !ignoreDiagonalLines)
                    coveredPoints[point] = coveredPoints.GetValueOrDefault(point) + 1;
        }

        return coveredPoints.Where(kvp => kvp.Value >= 2).Count();
    }

    private static IEnumerable<Point> GetStraightLine(Point point1, Point point2)
    {
        if (point1.X == point2.X)
        {
            var count = Math.Abs(point1.Y - point2.Y) + 1;
            var xRange = Enumerable.Repeat(point1.X, count);

            var yStart = Math.Min(point1.Y, point2.Y);
            var yRange = Enumerable.Range(yStart, count);

            return xRange.Zip(yRange, (x, y) => new Point(x,y));
        }
        else if (point1.Y == point2.Y)
        {
            var count = Math.Abs(point1.X - point2.X) + 1;
            var yRange = Enumerable.Repeat(point1.Y, count);

            var xStart = Math.Min(point1.X, point2.X);
            var xRange = Enumerable.Range(xStart, count);

            return xRange.Zip(yRange, (x, y) => new Point(x,y));
        }
        else 
        {
            return new List<Point>();
        }
    }

    private static IEnumerable<Point> GetDiagonalLine(Point point1, Point point2)
    {   // Coefficients for linear equation
        var k = (point1.Y - point2.Y) / (point1.X - point2.X);
        var m = -(k*point1.X - point1.Y);

        var xStart = Math.Min(point1.X, point2.X);
        var count = Math.Abs(point1.X - point2.X) + 1;
        var xRange = Enumerable.Range(xStart, count);

        return xRange.Select(x => new Point(x, k*x + m));
    }

    public static void Part1()
        => Console.WriteLine($"Part 1: {GetNumberOfOverlappingPoints(GetInput())}");

    public static void Part2()
        => Console.WriteLine($"Part 2: {GetNumberOfOverlappingPoints(GetInput(), false)}");
}