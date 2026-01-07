using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day07 : IAocDay
{
    public int Year => 2025;
    public int Day => 7;
    public string Title => "Laboratories - Beam Splitters";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day07()
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
        var result = CompletePaths(input);

        return $"Total splits: {result.totalSplits} ";
    }

    private string RunPart2(string input)
    {
        var result = CompletePaths(input);

        var grid = Helper.ToCharMatrix(result.matrix);
        var startPos = Helper.FindInMatrix(result.matrix, "S");
        long totalPaths = CountPathsToBottomExits(grid, startPos.Value.row, startPos.Value.col);

        return $"Total paths: {totalPaths} ";
    }

    // ---- Path counting (fast, matches rules) ----
    // - From 'S' or '|' you only go DOWN.
    // - From '^' you can go LEFT, RIGHT, and DOWN.
    // - Endpoint: any walkable cell on the bottom row counts as 1 path.
    private static long CountPathsToBottomExits(char[,] grid, int sr, int sc)
    {
        var memo = new Dictionary<(int r, int c), long>();
        return CountFrom(grid, sr, sc, memo);
    }

    private static long CountFrom(char[,] grid, int r, int c, Dictionary<(int r, int c), long> memo)
    {
        if (!InBounds(grid, r, c) || !IsWalkable(grid[r, c]))
            return 0;

        int lastRow = grid.GetLength(0) - 1;
        if (r == lastRow)
            return 1;

        if (memo.TryGetValue((r, c), out var cached))
            return cached;

        long total = 0;
        char ch = grid[r, c];

        // Always allow DOWN
        int nr = r + 1;
        int nc = c;
        if (InBounds(grid, nr, nc) && IsWalkable(grid[nr, nc]))
            total += CountFrom(grid, nr, nc, memo);

        // Only splitters can branch left/right
        if (ch == '^')
        {
            // LEFT
            nr = r;
            nc = c - 1;
            if (InBounds(grid, nr, nc) && IsWalkable(grid[nr, nc]))
                total += CountFrom(grid, nr, nc, memo);

            // RIGHT
            nr = r;
            nc = c + 1;
            if (InBounds(grid, nr, nc) && IsWalkable(grid[nr, nc]))
                total += CountFrom(grid, nr, nc, memo);
        }

        memo[(r, c)] = total;
        return total;
    }

    private static bool IsWalkable(char ch) =>
        ch == '|' || ch == 'S' || ch == '^';

    private static bool InBounds(char[,] grid, int r, int c)
        => r >= 0 && c >= 0 && r < grid.GetLength(0) && c < grid.GetLength(1);

    private ReturnHelper CompletePaths(string input)
    {
        long totalSplits = 0;
        var matrix = Helper.LoadMatrix(input, '\n', '1');
        var rowCount = matrix.GetLength(0);
        var columnCount = matrix.GetLength(1);
        var startPos = Helper.FindInMatrix(matrix, "S");
        List<(int row, int col)> beams = new List<(int row, int col)>();

        beams.Add((startPos.Value.row + 1, startPos.Value.col));
        matrix[startPos.Value.row + 1, startPos.Value.col] = "|";

        for (int r = startPos.Value.row + 1; r < rowCount; r++)
        {
            //search for existing beams ('|') in this row
            var currentBeams = beams.Where(b => b.row == r).ToList();

            foreach (var beam in currentBeams)
            {
                int row = beam.row + 1;
                int col = beam.col;

                if (row >= rowCount)
                    continue;

                string cell = matrix[row, col];

                if (cell == "^")
                {
                    totalSplits += 1;
                    if (matrix[row, col - 1] == ".")
                    {
                        beams.Add((row, col - 1));
                        matrix[row, col - 1] = "|";
                    }
                    if (matrix[row, col + 1] == ".")
                    {
                        beams.Add((row, col + 1));
                        matrix[row, col + 1] = "|";
                    }
                }
                else
                {
                    if (matrix[row, col] == ".")
                    {
                        beams.Add((row, col));
                        matrix[row, col] = "|";
                    }
                }
            }
            //Console.WriteLine(Helper.PrintMatrix(matrix));
        }
        Console.WriteLine(Helper.PrintMatrix(matrix));
        return new ReturnHelper { matrix = matrix, totalSplits = totalSplits };
    }
}

public class ReturnHelper
{
    public string[,] matrix { get; set; }
    public long totalSplits { get; set; }
}