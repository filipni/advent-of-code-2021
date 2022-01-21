using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Day9
{
    private record Point(int X, int Y, int Height, bool IsLowPoint);

    private static Point[][] Grid { get; }

    private static readonly int GridWidth;
    private static readonly int GridHeight;

    static Day9()
    {
        var input = File.ReadAllLines("input/day9.txt").Select(ParseRow).ToArray();
        GridHeight = input.Length;
        GridWidth = input[0].Length;

        Point[][] grid = new Point[GridHeight][].Select(x => new Point[GridWidth]).ToArray();

        for (int x = 0; x < GridHeight; x++)
            for (int y = 0; y < GridWidth; y++)
            {
                var currentHeight = input[x][y];
                var isLowPoint =
                    (x == 0 || currentHeight < input[x-1][y])
                    && (x == GridHeight - 1 || currentHeight < input[x+1][y])
                    && (y == 0 || currentHeight < input[x][y-1])
                    && (y == GridWidth - 1 || currentHeight < input[x][y+1]);
                
                grid[x][y] = new Point(x, y, currentHeight, isLowPoint);
            } 

        Grid = grid;
    }

    private static int[] ParseRow(string row) => row.Select(ParseChar).ToArray();
    private static int ParseChar(char c) => int.Parse(c.ToString());

    public static void Part1()
    {
        var sumOfRiskLevels = Grid.SelectMany(row => row.Where(point => point.IsLowPoint))
                                  .Sum(point => point.Height + 1);
        Console.WriteLine($"Part 1: {sumOfRiskLevels}");
    }

    public static void Part2()
    {
        Dictionary<Point, int> basins = new();

        for (int x = 0; x < GridHeight; x++)
            for (int y = 0; y < GridWidth; y++)
            {
                var lowPointFound = GetBasinLowPoint(Grid, Grid[x][y], out var lowPoint);
                if (!lowPointFound) continue;
                basins[lowPoint] = basins.GetValueOrDefault(lowPoint) + 1;
            }

        var product = basins.Select(kvp => kvp.Value)
                            .OrderByDescending(size => size)
                            .Take(3)
                            .Aggregate((x, y) => x * y);
        Console.WriteLine($"Part 2: {product}");
    }

    private static bool GetBasinLowPoint(Point[][] grid, Point point, out Point lowPoint)
    {
        var visitedPoints = new HashSet<Point>();
        
        Queue<Point> pointsToVisit = new();
        pointsToVisit.Enqueue(point);

        while (pointsToVisit.Any())
        {
            var currentPoint = pointsToVisit.Dequeue();
            
            if (visitedPoints.Contains(currentPoint))
                continue;
            
            visitedPoints.Add(currentPoint);

            if (currentPoint.Height == 9) continue;
            if (currentPoint.IsLowPoint)
            {
                lowPoint = currentPoint;
                return true;
            }

            var x = currentPoint.X;
            var y = currentPoint.Y;

            if (x > 0) pointsToVisit.Enqueue(grid[x - 1][y]);
            if (x < GridHeight - 1) pointsToVisit.Enqueue(grid[x + 1][y]);
            if (y > 0) pointsToVisit.Enqueue(grid[x][y - 1]);
            if (y < GridWidth - 1) pointsToVisit.Enqueue(grid[x][y + 1]);
        }

        lowPoint = null;
        return false;
    }
}