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
        var xStart = Math.Min(point1.X, point2.X);
        var xEnd = Math.Max(point1.X, point2.X);
        var yStart = Math.Min(point1.Y, point2.Y);
        var yEnd = Math.Max(point1.Y, point2.Y);

        var count = Math.Max(xEnd - xStart, yEnd - yStart) + 1;

        var xRange = point1.X == point2.X
            ? Enumerable.Repeat(point1.X, count)
            : Enumerable.Range(xStart, count);
        var yRange = point1.Y == point2.Y
            ? Enumerable.Repeat(point1.Y, count)
            : Enumerable.Range(yStart, count);

        return xRange.Zip(yRange, (x, y) => new Point(x,y));
    }

    private static IEnumerable<Point> GetDiagonalLine(Point point1, Point point2)
    {
        var startPoint = point1.X < point2.X ? point1 : point2;
        var endPoint = point1.X > point2.X ? point1 : point2;

        var count = endPoint.X - startPoint.X + 1;
        var xRange = Enumerable.Range(startPoint.X, count);

        var yRange = endPoint.Y > startPoint.Y
            ? Enumerable.Range(startPoint.Y, count)
            : Enumerable.Range(endPoint.Y, count).Reverse();

        return xRange.Zip(yRange, (x, y) => new Point(x, y));
    }

    public static void Part1()
        => Console.WriteLine($"Part 1: {GetNumberOfOverlappingPoints(GetInput())}");

    public static void Part2()
        => Console.WriteLine($"Part 2: {GetNumberOfOverlappingPoints(GetInput(), false)}");
}