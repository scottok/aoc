using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;
using static Aoc.Days.Year2025.Helper;

namespace Aoc.Days.Year2025;

public sealed class Day09 : IAocDay
{
    public int Year => 2025;
    public int Day => 9;
    public string Title => "Movie Theater = Tiles";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day09()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        long product = 1;

        //load input and process
        


        return $"Answer: {product}";
    }

    private string RunPart2(string input)
    {
        long samenessNumber = 42;
        return $"Answer: {samenessNumber}";
    }


}

