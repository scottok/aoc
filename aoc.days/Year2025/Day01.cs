using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day01 : IAocDay
{
    public int Year => 2025;
    public int Day => 1;
    public string Title => "Find the Password";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day01()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        string[] lines = input.Split('\n');
        int curr_pos = 50;
        string direction;
        int distance;
        int zero_cntr = 0;

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                direction = line.Substring(0, 1);
                distance = int.Parse(line.Substring(1));
                if (distance > 100) 
                    distance = distance % 100;

                switch (direction)
                {
                    case "R":
                        curr_pos += distance;
                        //curr_pos = curr_pos % 100;

                        if (curr_pos > 100)
                        {
                            curr_pos = curr_pos - 100;
                        }
                        break;
                    case "L":
                        curr_pos -= distance;

                        if (curr_pos < 0)
                        {
                            curr_pos = 100 + curr_pos;
                        }
                        break;
                    default:
                        System.Console.WriteLine("Invalid direction: " + direction);
                        break;
                }
                if (curr_pos == 100 || curr_pos == 0)
                {
                    zero_cntr++;
                }
            }
        }

        return "The password is: " + zero_cntr;
    }

    private string RunPart2(string input)
    {
        string[] lines = input.Split('\n');
        int curr_pos = 50;
        string direction;
        int distance;
        int fullRotations = 0;
        int zero_cntr = 0;
        bool zeroFlag = false;

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                direction = line.Substring(0, 1);
                distance = int.Parse(line.Substring(1));
                if (distance > 100)
                {
                    fullRotations += distance / 100;
                    distance = distance % 100;
                }

                switch (direction)
                {
                    case "R":
                        curr_pos += distance;
                        //curr_pos = curr_pos % 100;

                        if (curr_pos > 100)
                        {
                            curr_pos = curr_pos - 100;
                            if (!zeroFlag)
                                zero_cntr++;
                        }
                        break;
                    case "L":
                        curr_pos -= distance;

                        if (curr_pos < 0)
                        {
                            curr_pos = 100 + curr_pos;
                            if (!zeroFlag)
                                zero_cntr++;
                        }
                        break;
                    default:
                        System.Console.WriteLine("Invalid direction: " + direction);
                        break;
                }
                if (curr_pos == 100 || curr_pos == 0)
                {
                    zero_cntr++;
                    zeroFlag = true;
                }
                else
                {
                    zeroFlag = false;
                }
            }
        }

        return "The password is: " + (zero_cntr + fullRotations);
    }
}