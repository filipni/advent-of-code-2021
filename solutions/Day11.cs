using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class Day11
{
    private static int[,] Input { get; }

    private const int GridSize = 10;
    private const int FlashLevel = 10;

    static Day11()
    {
        var rawInput = File.ReadAllLines("input/day11.txt");
        var input = new int[GridSize, GridSize];

        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
                input[i, j] = int.Parse(rawInput[i][j].ToString()); 

        Input = input;
    }

    public static void Solution()
    {
        var energyLevels = Input;
        var step = 1;
        var numberOfFlashes = 0;
        var allHasFlashed = false;

        while (step <= 100 || !allHasFlashed)
        {
            var hasFlashed = new bool[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
                for (int j = 0; j < GridSize; j++)
                {
                    if (hasFlashed[i, j]) continue;
                    var newEnergyLevel = energyLevels[i, j] + 1;

                    if (newEnergyLevel >= FlashLevel)
                        numberOfFlashes += FlashOctupus(energyLevels, hasFlashed, i, j);
                    else
                        energyLevels[i, j] = newEnergyLevel;
                }

            allHasFlashed = true;
            for (int i = 0; i < GridSize; i++)
                for (int j = 0; j < GridSize; j++)
                    allHasFlashed = allHasFlashed && hasFlashed[i, j];

            if (allHasFlashed)
                Console.WriteLine($"Part 2: {step}");

            if (step == 100)
                Console.WriteLine($"Part 1: {numberOfFlashes}");

            step++;
        }
    }

    private static int FlashOctupus(int[,] energyLevels, bool[,] hasFlashed, int x, int y)
    {
        hasFlashed[x, y] = true;
        energyLevels[x, y] = 0;
        var numberOfFlashes = 1;

        for (int i = x - 1; i <= x + 1; i++)
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y) continue;
                if (IsOutOfBounds(i, j) || hasFlashed[i, j]) continue;

                var newEnergyLevel = energyLevels[i, j] + 1;
                if (newEnergyLevel >= FlashLevel)
                    numberOfFlashes += FlashOctupus(energyLevels, hasFlashed, i, j);
                else
                    energyLevels[i, j] = newEnergyLevel;
            }

        return numberOfFlashes;
    }

    private static bool IsOutOfBounds(int x, int y)
        => x < 0 || y < 0 || x > GridSize - 1 || y > GridSize - 1;
}