using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day15
{
    private record Vertex(int X, int Y);
    private record Edge(Vertex V1, Vertex V2);
    private record Graph(List<Vertex> Vertices, Dictionary<Edge, int> Edges);

    private static Graph CreateGraph(int repeats = 1)
    {
        int[][] riskLevels = File.ReadAllLines("input/day15.txt").Select(ParseDigits).ToArray();

        var height = riskLevels.Length;
        var width = riskLevels[0].Length;

        var extendedHeight = height * repeats;
        var extendedWidth = width * repeats;

        int[][] extendedRiskLevels = new int[extendedHeight][];
        for (int i = 0; i < extendedHeight; i++)
        {
            extendedRiskLevels[i] = new int[extendedWidth];
            for (int j = 0; j < extendedWidth; j++)
            {
                var riskLevel = riskLevels[i % height][j % width] + i / height + j / width;
                extendedRiskLevels[i][j] = riskLevel >= 10 ? riskLevel % 10 + 1 : riskLevel;
            }
        }

        riskLevels = extendedRiskLevels;
        width = extendedWidth;
        height = extendedWidth;

        List<Vertex> vertices = new();
        Dictionary<Edge, int> edges = new();

        List<(int, int)> positions = new();
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                positions.Add((i, j));

        foreach (var (x, y) in positions)
        {
            var vertex = new Vertex(x, y);
            vertices.Add(vertex);

            if (x > 0) 
            {
                var edge = new Edge(vertex, new Vertex(x - 1, y));
                edges[edge] = riskLevels[y][x - 1];  
            }
            if (x < width - 1)
            {
                var edge = new Edge(vertex, new Vertex(x + 1, y));
                edges[edge] = riskLevels[y][x + 1];
            }
            if (y > 0)
            {
                var edge = new Edge(vertex, new Vertex(x, y - 1));
                edges[edge] = riskLevels[y - 1][x];
            }
            if (y < height - 1)
            {
                var edge = new Edge(vertex, new Vertex(x, y + 1));
                edges[edge] = riskLevels[y + 1][x] ;
            }
        }

        return new Graph(vertices, edges);
    }

    private static int[] ParseDigits(string digits)
        => digits.Select(digit => int.Parse(digit.ToString())).ToArray();

    public static void Part1()
    {
        var graph = CreateGraph();
        var source = new Vertex(0, 0);
        var (dist, prev) = Dijkstra(graph, source);

        var destination = graph.Vertices.OrderByDescending(vertix => vertix.X + vertix.Y).First();
        System.Console.WriteLine($"Part 1: {dist[destination]}");
    }

    public static void Part2()
    {
        var graph = CreateGraph(5);
        var source = new Vertex(0, 0);
        var (dist, prev) = Dijkstra(graph, source);

        var destination = graph.Vertices.OrderByDescending(vertix => vertix.X + vertix.Y).First();
        System.Console.WriteLine($"Part 2: {dist[destination]}");
    }

    private static (Dictionary<Vertex, int> dist, Dictionary<Vertex, Vertex> prev) Dijkstra(Graph graph, Vertex source)
    {
        var q = new HashSet<Vertex>();
        Dictionary<Vertex, int> dist = new(); 
        Dictionary<Vertex, Vertex> prev = new();

        foreach (var vertex in graph.Vertices)
        {
            dist[vertex] = int.MaxValue;
            prev[vertex] = null;
            q.Add(vertex);
        }

        dist[source] = 0;

        while (q.Any())
        {
            var u = q.OrderBy(v => dist[v]).First();

            q.Remove(u);

            var neighbours = graph.Edges.Keys.Where(e => e.V1 == u)
                                             .Select(e => e.V2)
                                             .Where(v => q.Contains(v));

            foreach (var v in neighbours)
            {
                var alt = dist[u] + graph.Edges[new Edge(u, v)];
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        return (dist, prev);
    }
}