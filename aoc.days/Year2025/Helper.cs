using System.Numerics;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public static class Helper
{
    public  readonly record struct Point3D(long X, long Y, long Z);
    public static IEnumerable<string> SplitByLength(string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            yield break;

        for (int i = 0; i < input.Length; i += length)
        {
            yield return input.Substring(i, Math.Min(length, input.Length - i));
        }
    }

    private static string[] SplitFixedWidth(string input, int width)
    {
        if (width <= 0)
            throw new ArgumentException("Fixed width must be greater than zero.");

        int columnCount = (int)Math.Ceiling(input.Length / (double)width);
        var result = new string[columnCount];

        for (int i = 0; i < columnCount; i++)
        {
            int start = i * width;
            int length = Math.Min(width, input.Length - start);

            result[i] = input.Substring(start, length).Trim();
        }

        return result;
    }

    public static char[,] LoadCharMatrix(string input, char rowDelimiter = '\n', char columnDelimiter = ' ')
    {
        var rows = input
            .Split(rowDelimiter, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim())
            .ToArray();

        if (rows.Length == 0)
            throw new ArgumentException("Input is empty.");

        bool isFixedWidth = char.IsDigit(columnDelimiter);
        int fixedWidth = isFixedWidth ? int.Parse(columnDelimiter.ToString()) : 0;

        string[] firstRowColumns = isFixedWidth
            ? SplitFixedWidth(rows[0], fixedWidth)
            : rows[0].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries);

        int rowCount = rows.Length;
        int columnCount = firstRowColumns.Length;

        var matrix = new char[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] columns = isFixedWidth
                ? SplitFixedWidth(rows[i], fixedWidth)
                : rows[i].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries);

            if (columns.Length != columnCount)
                throw new FormatException($"Row {i} has {columns.Length} columns, expected {columnCount}.");

            for (int j = 0; j < columnCount; j++)
            {
                if (columns[j].Length != 1)
                    throw new FormatException(
                        $"Invalid char at row {i}, column {j}: '{columns[j]}'");

                matrix[i, j] = columns[j][0];
            }
        }

        return matrix;
    }

    public static string[,] LoadMatrix(string input, char rowDelimiter = '\n', char columnDelimiter = ' ')
    {
        var rows = input
            .Split(rowDelimiter, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim())
            .ToArray();

        if (rows.Length == 0)
            throw new ArgumentException("Input is empty.");

        bool isFixedWidth = char.IsDigit(columnDelimiter);
        int fixedWidth = isFixedWidth ? int.Parse(columnDelimiter.ToString()) : 0;

        string[] firstRowColumns = isFixedWidth
            ? SplitFixedWidth(rows[0], fixedWidth)
            : rows[0].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries);

        int rowCount = rows.Length;
        int columnCount = firstRowColumns.Length;

        var matrix = new string[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] columns = isFixedWidth
                ? SplitFixedWidth(rows[i], fixedWidth)
                : rows[i].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries);

            if (columns.Length != columnCount)
                throw new FormatException($"Row {i} has {columns.Length} columns, expected {columnCount}.");

            for (int j = 0; j < columnCount; j++)
                matrix[i, j] = columns[j];
        }

        return matrix;
    }

    // find value in matrix
    public static (int row, int col)? FindInMatrix(string[,] matrix, string value)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (matrix[r, c] == value)
                    return (r, c);
            }
        }
        return null;
    }

public static char[,] ToCharMatrix(string[,] source)
{
    int rows = source.GetLength(0);
    int cols = source.GetLength(1);

    var result = new char[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if (source[i, j].Length != 1)
                throw new ArgumentException($"Cell [{i},{j}] is not a single character.");

            result[i, j] = source[i, j][0];
        }
    }

    return result;
}

    //return a 2D string array
    public static string PrintMatrix(string[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        var result = new System.Text.StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result.Append(matrix[i, j]);
            }
            result.AppendLine();
        }

        return result.ToString();
    }

    public static int CountAdjacentSame8(string[,] grid, int r, int c)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        string v = grid[r, c];
        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            int rr = r + dr[i], cc = c + dc[i];
            if (rr >= 0 && rr < rows && cc >= 0 && cc < cols && grid[rr, cc] == v)
                count++;
        }
        return count;
    }

    public static long DistanceManhattan((int row, int col) pos1, (int row, int col) pos2)
    {
        return Math.Abs(pos1.row - pos2.row) + Math.Abs(pos1.col - pos2.col);
    }

    public static long DistanceChebyshev((int row, int col) pos1, (int row, int col) pos2)
    {
        return Math.Max(Math.Abs(pos1.row - pos2.row), Math.Abs(pos1.col - pos2.col));
    }

    public static long DistanceEuclidean((long row, long col) pos1, (long row, long col) pos2)
    {
        long dr = pos1.row - pos2.row;
        long dc = pos1.col - pos2.col;
        return (long)Math.Sqrt(dr * dr + dc * dc);
    }

    public static long Distance3DManhattan((long x, long y, long z) pos1, (long x, long y, long z) pos2)
    {
        return Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y) + Math.Abs(pos1.z - pos2.z);
    }   

    public static long Distance3DChebyshev((int x, int y, int z) pos1, (int x, int y, int z) pos2)
    {
        return Math.Max(Math.Max(Math.Abs(pos1.x - pos2.x), Math.Abs(pos1.y - pos2.y)), Math.Abs(pos1.z - pos2.z));
    }

    public static long Distance3DEuclidean(Point3D pos1, Point3D pos2)
    {
        long dx = pos1.X - pos2.X;
        long dy = pos1.Y - pos2.Y;
        long dz = pos1.Z - pos2.Z;
        return (long)Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

}