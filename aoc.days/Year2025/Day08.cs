using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;
using static Aoc.Days.Year2025.Helper;

namespace Aoc.Days.Year2025;

public sealed class Day08 : IAocDay
{
    public int Year => 2025;
    public int Day => 8;
    public string Title => "Playground - junction boxes";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day08()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }


    private static readonly (int dr, int dc)[] Dirs =
    {
        (-1, 0), (1, 0), (0, -1), (0, 1)
    };


    private string RunPart1(string input)
    {
        var distances = new Dictionary<(Point3D A, Point3D B), double>();
        var junctions = new Dictionary<Point3D, List<Point3D>>();

        // parse points
        var points = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Select(p =>
            {
                var coords = p.Split(',', StringSplitOptions.RemoveEmptyEntries);

                return new Point3D(
                    X: int.Parse(coords[0]),
                    Y: int.Parse(coords[1]),
                    Z: int.Parse(coords[2])
                );
            })
            .ToArray();

        // calculate distances between points
        foreach (var point in points)
        {
            foreach (var point2 in points)
            {
                if (point == point2)
                    continue;
                var distance = Distance3DEuclidean(point, point2);
                if (!distances.ContainsKey((point, point2)) && !distances.ContainsKey((point2, point)))
                    distances[(point, point2)] = distance;
            }
        }

        var sortedDistances = distances.OrderBy(kv => kv.Value);



        return $"Total splits: {distances.Count} ";
    }

    private string RunPart2(string input)
    {


        return $"Total paths: {'j'} ";
    }


}

public class JunctionBox
{
    public Point3D Position { get; }
    public List<JunctionBox> Connections { get; }

    public JunctionBox(Point3D position)
    {
        Position = position;
        Connections = new List<JunctionBox>();
    }
}