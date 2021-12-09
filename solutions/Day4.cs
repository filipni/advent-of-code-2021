using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Day4
{
    private List<int> DrawnNumbers { get; }
    private List<int[][]> BingoBoards { get; }

    private const int BoardSize = 5;

    public Day4()
    {
        var rawInput = File.ReadAllText("input/day4.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
        DrawnNumbers = rawInput[0].Split(',').Select(x => int.Parse(x)).ToList();

        var bingoBoards = new List<int[][]>();
        for (int i = 1; i < rawInput.Length; i++)
        {
            var board = new int[BoardSize][];
            var rows = rawInput[i].Split($"{Environment.NewLine}");

            for (int j = 0; j < BoardSize; j++)
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

    private void MarkNumber(int[][] bingoBoard, int number)
    {
        for (int i = 0; i < BoardSize; i++)
            for (int j = 0; j < BoardSize; j++)
                if (bingoBoard[i][j] == number)
                    bingoBoard[i][j] = 'x';
    }

    private bool CheckForBingo(int[][] board)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            var row_marks = 0;
            var column_marks = 0;
            for (int j = 0; j < BoardSize; j++)
            {
                if (board[i][j] == 'x')
                    row_marks++;
                if (board[j][i] == 'x')
                    column_marks++;
            }

            if (row_marks == 5 || column_marks == 5) return true;
        }

        return false;
    }

    private int GetPoints(int[][] board)
        => board.Sum(row => row.Sum(number => number != 'x' ? number : 0));

    public void Part1()
    {
        foreach (var number in DrawnNumbers)
        {
            foreach (var board in BingoBoards)
            {
                MarkNumber(board, number);
                var bingo = CheckForBingo(board);

                if (bingo)
                {
                    var points = GetPoints(board);
                    System.Console.WriteLine($"Part 1: {points * number}");
                    return;
                }

            }
        }
    } 

    public void Part2()
    {
        var winningBoards = new HashSet<int>();

        foreach (var number in DrawnNumbers)
        {
            for (int i = 0; i < BingoBoards.Count; i++)
            {
                var board = BingoBoards[i];
                MarkNumber(board, number);
                var bingo = CheckForBingo(board);

                if (bingo)
                    winningBoards.Add(i);

                if (winningBoards.Count == BingoBoards.Count)
                {
                    var points = GetPoints(board);
                    System.Console.WriteLine($"Part 2: {points * number}");
                    return;
                }

            }
        }
    } 
}