using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day06 : IAocDay
{
    public int Year => 2025;
    public int Day => 6;
    public string Title => "Trash Compactor";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day06()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        long totalProducts = 0;

        var equations = Helper.LoadMatrix(input);

        int rows = equations.GetLength(0);
        int cols = equations.GetLength(1);

        for (int i = 0; i < cols; i++)
        {
            string operand = equations[rows - 1, i]; //get the operand from the last row
            long columnProduct = 0;

            for (int j = 0; j < rows - 1; j++) //exclude the last row, which contains the operands
            {

                if (long.TryParse(equations[j, i], out long number))
                {
                    if (operand == "+")
                    {
                        columnProduct += number;
                    }
                    else if (operand == "*")
                    {
                        if (j == 0)
                            columnProduct = number; //initialize for multiplication
                        else
                            columnProduct *= number;
                    }
                }
            }
            totalProducts += columnProduct;
        }
        return $"Total of equation answers: {totalProducts} ";
    }

    private string RunPart2(string input)
    {
        long totalProducts = 0;

        var equations = ParseByOperatorLineSpans(input, out int columnCount);

        int rows = equations.GetLength(0);
        int cols = equations.GetLength(1);


        return $"Total of equation answers: {totalProducts} ";
    }

    public static char[][][] ParseByOperatorLineSpans(string input, out int columnCount)
    {
        string[] lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // 1) Find operator line (first line that contains + or *)
        int opLineIndex = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            string s = lines[i];
            for (int j = 0; j < s.Length; j++)
            {
                if (s[j] == '+' || s[j] == '*')
                {
                    opLineIndex = i;
                    break;
                }
            }
            if (opLineIndex != -1) break;
        }

        if (opLineIndex == -1)
            throw new FormatException("Operator line not found.");

        string opLine = lines[opLineIndex];

        // 2) Collect operator indices
        var ops = new List<int>(1024);
        for (int i = 0; i < opLine.Length; i++)
        {
            char ch = opLine[i];
            if (ch == '+' || ch == '*')
                ops.Add(i);
        }

        if (ops.Count == 0)
            throw new FormatException("Operator line contained no '+' or '*'.");

        columnCount = ops.Count ;

        // 3) Build column spans [start, end] based on operator positions
        int[] starts = new int[columnCount + 1];
        int[] widths = new int[columnCount + 1];

        // col0: from 0 to before first operator
        starts[0] = 0;
        widths[0] = ops[0] - 0; // may be 0 if operator at index 0

        // middle columns: between operators (excluding the operator char itself)
        for (int c = 1; c < ops.Count ; c++)
        {
            starts[c] = ops[c - 1] ;
            widths[c] = ops[c] - starts[c];
            if (widths[c] < 0) widths[c] = 0;
        }

        // last column: after last operator to end of operator line
        starts[columnCount] = ops[ops.Count - 1] ;
        widths[columnCount] = opLine.Length - starts[columnCount - 1];
        if (widths[columnCount - 1] < 0) widths[columnCount - 1] = 0;

        //clear index 0.
        for (int i = 0; i < starts.Length - 1; i++)
        {
            starts[i] = starts[i + 1];
            widths[i] = widths[i + 1];
        }
        starts[starts.Length - 1] = 0; // optional: clear last slot
        widths[widths.Length - 1] = 0; // optional: clear last slot

        // 4) Slice each data line by those spans (pad right to opLine length)
        var dataLines = new List<string>(lines.Length - 1);
        for (int i = 0; i < lines.Length; i++)
            if (i != opLineIndex)
                dataLines.Add(lines[i]);

        char[][][] result = new char[dataLines.Count][][];

        for (int r = 0; r < dataLines.Count; r++)
        {
            string line = dataLines[r];

            if (line.Length < opLine.Length)
                line = line.PadRight(opLine.Length, ' ');

            result[r] = new char[columnCount][];

            for (int c = 0; c < columnCount - 1; c++)
            {
                int w = widths[c];

                // represent empty columns as a single-space cell so they still exist
                if (w <= 0) w = 1;

                char[] cell = new char[w];
                for (int k = 0; k < w; k++) cell[k] = ' '; // baseline spaces (no '\0')

                int s = starts[c] ;
                int copyLen = Math.Min(w, Math.Max(0, line.Length - s));

                for (int k = 0; k < copyLen ; k++)
                    cell[k] = line[s + k];

                result[r][c] = cell;
            }
        }

        return result;
    }
}