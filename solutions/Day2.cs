using System;
using System.IO;
using System.Linq;

public class Day2
{
    private record Position(int Horizontal = 0, int Depth = 0, int Aim = 0);

    private enum Direction { forward, up, down }

    private static string[] Input => File.ReadAllLines("input/day2.txt");

    public static void Part1()
    {
        var subPosition = RunCommands(Input, MovePosition);
        System.Console.WriteLine($"Part 1: {subPosition.Horizontal * subPosition.Depth}");
    }

    public static void Part2()
    {
        var subPosition = RunCommands(Input, MovePositionWithAim);
        System.Console.WriteLine($"Part 2: {subPosition.Horizontal * subPosition.Depth}");
    }

    private static Position RunCommands(string[] commands, Func<Position, Direction, int, Position> Translate)
    {
        Position subPosition = new();

        foreach (var instruction in commands)
        {
            var split = instruction.Split();
            Direction.TryParse(split[0], out Direction direction);
            var units = int.Parse(split[1]);

            subPosition = Translate(subPosition, direction, units);
        }

        return subPosition;
    }

    private static Position MovePosition(Position subPosition, Direction direction, int units)
        => direction switch
        {
            Direction.forward => subPosition with { Horizontal = subPosition.Horizontal + units },
            Direction.down => subPosition with { Depth = subPosition.Depth + units },
            Direction.up => subPosition with { Depth = subPosition.Depth - units },
            _ => subPosition,
        };

    private static Position MovePositionWithAim(Position subPosition, Direction direction, int units)
        => direction switch
        {
            Direction.forward => subPosition with
            {
                Horizontal = subPosition.Horizontal + units,
                Depth = subPosition.Depth + subPosition.Aim * units
            },
            Direction.down => subPosition with { Aim = subPosition.Aim + units },
            Direction.up => subPosition with { Aim = subPosition.Aim - units },
            _ => subPosition,
        };
}