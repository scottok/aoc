using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day05 : IAocDay
{
    public int Year => 2025;
    public int Day => 5;
    public string Title => "Cafeteria";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day05()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        int totalFresh = 0;
        var inventory = LoadInventory(input);
        foreach (var ingredient in inventory.Ingredients)
        {
            foreach (var range in inventory.FreshRanges)
            {
                if (ingredient.Id >= range.Start && ingredient.Id <= range.End)
                {
                    totalFresh++;
                    ingredient.IsFresh = true;
                    break;
                }
            }
        }

        return $"Total Fresh Food Items: {totalFresh} ";
    }

    private string RunPart2(string input)
    {
        long totalFresh = 0;
        var inventory = LoadInventory(input);

        totalFresh = CountFreshIds(inventory.FreshRanges.Select(r => (r.Start, r.End)));

        return $"Total Fresh Food IDs: {totalFresh} ";
    }

    private Inventory LoadInventory(string input)
    {
        var inventory = new Inventory();
        List<FreshRange> freshRanges = new List<FreshRange>();
        List<Ingredient> ingredients = new List<Ingredient>();

        string[] lines = input.Split('\n');
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                if (line.Contains("-"))
                {
                    var parts = line.Split('-');
                    freshRanges.Add(new FreshRange()
                    {
                        Start = Int64.Parse(parts[0]),
                        End = Int64.Parse(parts[1])
                    });
                }
                else
                {
                    ingredients.Add(new Ingredient()
                    {
                        Id = Int64.Parse(line)
                    });
                }
            }
        }
        inventory.FreshRanges = freshRanges;
        inventory.Ingredients = ingredients;

        return inventory;
    }

    public long CountFreshIds(IEnumerable<(long Start, long End)> ranges)
    {
        var ordered = ranges
            .OrderBy(r => r.Start)
            .ToList();

        if (ordered.Count == 0)
            return 0;

        long total = 0;
        long currentStart = ordered[0].Start;
        long currentEnd = ordered[0].End;

        foreach (var range in ordered.Skip(1))
        {
            if (range.Start <= currentEnd + 1)
            {
                // overlap or adjacent
                currentEnd = Math.Max(currentEnd, range.End);
            }
            else
            {
                total += (currentEnd - currentStart + 1);
                currentStart = range.Start;
                currentEnd = range.End;
            }
        }

        // add final range
        total += (currentEnd - currentStart + 1);

        return total;
    }
}

internal class Inventory()
{
    public List<FreshRange> FreshRanges { get; set; } = new List<FreshRange>();
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
internal class FreshRange()
{
    public Int64 Start { get; set; }
    public Int64 End { get; set; }
}
internal class Ingredient()
{
    public Int64 Id { get; set; }
    public bool IsFresh { get; set; }
}