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

    private string RunPart1(string input)
    {
        var distances = new Dictionary<(Point3D A, Point3D B), double>();
        double MAX_ITERATIONS = 1000;

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

        // sort distances
        var sortedDistances = distances.OrderBy(kv => kv.Value);

        //find all circuits with closest distances
        long circuitCount = 0;
        long cntInterations = 0;
        var circuits = new Dictionary<Point3D, long>();
        foreach (var (pointsPair, distance) in sortedDistances)
        {
            var (pointA, pointB) = pointsPair;
            
            if (!circuits.ContainsKey(pointA) && !circuits.ContainsKey(pointB))
            {                
                // create new circuit
                circuitCount++;
                circuits[pointA] = circuitCount;
                circuits[pointB] = circuitCount;
            }
            else if (circuits.ContainsKey(pointA) && !circuits.ContainsKey(pointB))
            {
                // add pointB to pointA's circuit
                circuits[pointB] = circuits[pointA];
            }
            else if (!circuits.ContainsKey(pointA) && circuits.ContainsKey(pointB))
            {
                // add pointA to pointB's circuit
                circuits[pointA] = circuits[pointB];
            }
            else if (circuits.ContainsKey(pointA) && circuits.ContainsKey(pointB))
            {
                var circuitA = circuits[pointA];
                var circuitB = circuits[pointB];
                if (circuitA != circuitB)
                {
                    // merge circuits
                    foreach (var key in circuits.Keys.ToArray())
                    {
                        if (circuits[key] == circuitB)
                        {
                            circuits[key] = circuitA;
                        }
                    }
                }
            }

            cntInterations++;
            if(cntInterations == MAX_ITERATIONS)
                break;
        }

        //get unique circuits with there counts of points
        var uniqueCircuits = circuits.Values.Distinct().ToArray();
        var circuitPointsCount = new Dictionary<long, int>();
        foreach (var circuitId in uniqueCircuits)
        {
            var count = circuits.Values.Count(c => c == circuitId);
            circuitPointsCount[circuitId] = count;
        }
        
        // get top 3 largest circuits and multiply their sizes
        var top3 = circuitPointsCount.Values.OrderByDescending(c => c).Take(3).ToArray();
        var product = top3.Aggregate(1, (a, b) => a * b);

        return $"Answer: {product}";
    }

    private string RunPart2(string input)
    {
        var distances = new Dictionary<(Point3D A, Point3D B), double>();
        double MAX_ITERATIONS = 10000;
        long samenessNumber = 42;

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

        // sort distances
        var sortedDistances = distances.OrderBy(kv => kv.Value);

        //find all circuits with closest distances
        long circuitCount = 0;
        long cntInterations = 0;
        var circuits = new Dictionary<Point3D, long>();
        foreach (var (pointsPair, distance) in sortedDistances)
        {
            var (pointA, pointB) = pointsPair;
            
            if (!circuits.ContainsKey(pointA) && !circuits.ContainsKey(pointB))
            {                
                // create new circuit
                circuitCount++;
                circuits[pointA] = circuitCount;
                circuits[pointB] = circuitCount;
            }
            else if (circuits.ContainsKey(pointA) && !circuits.ContainsKey(pointB))
            {
                // add pointB to pointA's circuit
                circuits[pointB] = circuits[pointA];
            }
            else if (!circuits.ContainsKey(pointA) && circuits.ContainsKey(pointB))
            {
                // add pointA to pointB's circuit
                circuits[pointA] = circuits[pointB];
            }
            else if (circuits.ContainsKey(pointA) && circuits.ContainsKey(pointB))
            {
                var circuitA = circuits[pointA];
                var circuitB = circuits[pointB];
                if (circuitA != circuitB)
                {
                    // merge circuits
                    foreach (var key in circuits.Keys.ToArray())
                    {
                        if (circuits[key] == circuitB)
                        {
                            circuits[key] = circuitA;
                        }
                    }
                }
            }

            //when all circuits values are the same, we can stop
            if (circuits.Values.Distinct().Count() == 1 && circuits.Values.Count >= points.Length) 
            {
                samenessNumber = pointA.X * pointB.X; // just to use pointA and pointB
                break;
            }
        }
        return $"Answer: {samenessNumber}";
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