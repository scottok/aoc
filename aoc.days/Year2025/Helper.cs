using System.Numerics;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public static class Helper
{
    public static IEnumerable<string> SplitByLength(string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            yield break;

        for (int i = 0; i < input.Length; i += length)
        {
            yield return input.Substring(i, Math.Min(length, input.Length - i));
        }
    }

    public static IEnumerable<int> SplitByLengthInt(string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            yield break;

        for (int i = 0; i < input.Length; i += length)
        {
            yield return int.Parse(input.Substring(i, Math.Min(length, input.Length - i)));
        }
    }

    public static string[,] LoadMatrix(string input, char rowDelimiter = '\n', char columnDelimiter = ' ')
    {
        var rows = input.Split(rowDelimiter, StringSplitOptions.RemoveEmptyEntries);
        int rowCount = rows.Length;
        int columnCount = rows[0].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries).Length;

        var matrix = new string[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {

            var columns = rows[i].Split(columnDelimiter, StringSplitOptions.RemoveEmptyEntries);
            //var columns = rows[i].ToCharArray().Select(c => c.ToString()).ToArray();
            for (int j = 0; j < columnCount; j++)
            {
                matrix[i, j] = columns[j];
            }
        }

        return matrix;
    }

public static char[][][] ParseFixedColumnsAutoWidth(string input)
{
    string[] lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

    // 1) Auto-detect column width as the longest contiguous run of digits anywhere
    int cellWidth = 0;

    for (int r = 0; r < lines.Length; r++)
    {
        string line = lines[r];

        int run = 0;
        for (int i = 0; i < line.Length; i++)
        {
            char ch = line[i];
            if (ch >= '0' && ch <= '9')
            {
                run++;
                if (run > cellWidth) cellWidth = run;
            }
            else
            {
                run = 0;
            }
        }
    }

    if (cellWidth == 0)
        return Array.Empty<char[][]>();

    const char delimiter = ' '; // one space between columns in the fixed format

    // 2) Parse each line into fixed-width cells (cellWidth), skipping ONE delimiter between cells
    char[][][] result = new char[lines.Length][][];

    for (int r = 0; r < lines.Length; r++)
    {
        string line = lines[r];

        // Count columns by walking fixed blocks
        int idx = 0;
        int colCount = 0;

        while (idx < line.Length)
        {
            colCount++;
            idx += cellWidth;
            if (idx < line.Length && line[idx] == delimiter)
                idx += 1; // skip exactly one delimiter space
        }

        result[r] = new char[colCount][];

        idx = 0;
        for (int c = 0; c < colCount; c++)
        {
            char[] cell = new char[cellWidth];

            // Fill with spaces so trailing blanks are real spaces (not '\0')
            for (int k = 0; k < cellWidth; k++)
                cell[k] = ' ';

            // Copy up to cellWidth chars from the line
            for (int k = 0; k < cellWidth; k++)
            {
                int pos = idx + k;
                if (pos < line.Length)
                    cell[k] = line[pos];
            }

            result[r][c] = cell;

            idx += cellWidth;
            if (idx < line.Length && line[idx] == delimiter)
                idx += 1;
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
    int[] dr = { -1,-1,-1, 0,0, 1,1,1 };
    int[] dc = { -1, 0, 1,-1,1,-1,0,1 };

    int count = 0;
    for (int i = 0; i < 8; i++)
    {
        int rr = r + dr[i], cc = c + dc[i];
        if (rr >= 0 && rr < rows && cc >= 0 && cc < cols && grid[rr, cc] == v)
            count++;
    }
    return count;
}


}