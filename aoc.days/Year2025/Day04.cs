using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day04 : IAocDay
{
    public int Year => 2025;
    public int Day => 4;
    public string Title => "Printing Department";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day04()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        int MAX_ROLLS = 3;
        Int32 totalUsable = 0;
        var areaMap = Helper.LoadMatrix(input);
        var outputMap = areaMap.Clone() as string[,];

        // find all rolls (@) where there are no more that MAX_ROLLS adjacent rolls
        for (int i = 0; i < areaMap.GetLength(0); i++)
        {
            for (int j = 0; j < areaMap.GetLength(1); j++)
            {
                if (areaMap[i, j] == "@")
                {
                    if (Helper.CountAdjacentSame8(areaMap, i, j) <= MAX_ROLLS)
                    {
                        outputMap[i, j] = "X"; // mark as usable
                        totalUsable++;
                    }
                }
            }
        }
        return $"{Helper.PrintMatrix(outputMap)}\nTotal Usable Rolls: {totalUsable} ";
    }

    private string RunPart2(string input)
    {
        int MAX_ROLLS = 3;
        Int32 totalUsable = 0;
        Int32 iterationUsable = 0;
        var areaMap = Helper.LoadMatrix(input);
        var outputMap = areaMap.Clone() as string[,];

        do
        {
            // find all rolls (@) where there are no more that MAX_ROLLS adjacent rolls
            for (int i = 0; i < areaMap.GetLength(0); i++)
            {
                for (int j = 0; j < areaMap.GetLength(1); j++)
                {
                    if (areaMap[i, j] == "@")
                    {
                        if (Helper.CountAdjacentSame8(areaMap, i, j) <= MAX_ROLLS)
                        {
                            outputMap[i, j] = "X"; // mark as usable
                            iterationUsable++;
                        }
                    }
                }
            }
            //Console.WriteLine($"{Helper.PrintMatrix(outputMap)}\nThis Iterations Usable Rolls: {iterationUsable} ");
            totalUsable += iterationUsable;
             
            if (iterationUsable == 0)
            {
                break;
            }
            else
            {
                iterationUsable = 0;
                areaMap = outputMap.Clone() as string[,];
            }
            
        } while (true);

        return $"Total Usable Rolls: {totalUsable} ";
    }

}
