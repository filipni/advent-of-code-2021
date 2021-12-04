using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

public class Day4
{
    private List<int> DrawnNumbers { get; }
    private List<int[][]> BingoBoards { get; }

    public Day4()
    {
        var rawInput = File.ReadAllText("input/day4.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
        DrawnNumbers = rawInput[0].Split(',').Select(x => int.Parse(x)).ToList();

        var bingoBoards = new List<int[][]>();
        for (int i = 1; i < rawInput.Length; i++)
        {
            var board = new int[5][];
            var rows = rawInput[i].Split($"{Environment.NewLine}");

            for (int j = 0; j < 5; j++)
            {
                board[j] = rows[j]
                            .Split()
                            .Where(x => x != string.Empty)
                            .Select(x => int.Parse(x))
                            .ToArray();             
            }

            bingoBoards.Add(board);
        }

        BingoBoards = bingoBoards;
    }

    public void Part1()
    {
        Console.WriteLine($"Part 1: ");
    } 

    public void Part2()
    {
        Console.WriteLine($"Part 2: ");
    } 
}