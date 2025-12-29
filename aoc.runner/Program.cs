using System.Diagnostics;

if (args.Length == 0)
{
    PrintUsage();
    return 1;
}

// list [year]
if (args[0].Equals("list", StringComparison.OrdinalIgnoreCase))
{
    if (args.Length > 1 && int.TryParse(args[1], out var listYear))
        ListDays(listYear);
    else
        ListDays(null);

    return 0;
}

if (args.Length < 2)
{
    Console.WriteLine("Expected: <year> <day> [variant] [input]");
    return 2;
}

if (!int.TryParse(args[0], out var year) ||
    !int.TryParse(args[1], out var day))
{
    Console.WriteLine("Year and day must be numeric.");
    return 3;
}

var variant = args.Length >= 3 ? args[2] : "1";
var inputPath = args.Length >= 4
    ? args[3]
    : Path.Combine("inputs", year.ToString(), $"day{day:00}.txt");

if (!DayRegistry.Days.TryGetValue((year, day), out var solver))
{
    Console.WriteLine($"No solver registered for {year} Day {day:00}");
    return 4;
}

if (!solver.Variants.TryGetValue(variant, out var run))
{
    Console.WriteLine($"Variant '{variant}' not found.");
    Console.WriteLine($"Available: {string.Join(", ", solver.Variants.Keys)}");
    return 5;
}

if (!File.Exists(inputPath))
{
    Console.WriteLine($"Input not found: {inputPath}");
    return 6;
}

var input = await File.ReadAllTextAsync(inputPath);

Console.WriteLine($"{solver.Year} Day {solver.Day:00} — {solver.Title}");
Console.WriteLine($"Variant: {variant}");
Console.WriteLine($"Input: {inputPath}");
Console.WriteLine();

var sw = Stopwatch.StartNew();
var output = run(input);
sw.Stop();

Console.WriteLine(output);
Console.WriteLine();
Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");

return 0;

static void PrintUsage()
{
    Console.WriteLine("""
Usage:
  dotnet run -- <year> <day> [variant] [input]
  dotnet run -- list [year]
""");
}

static void ListDays(int? year)
{
    var days = DayRegistry.Days.Values
        .Where(d => year is null || d.Year == year)
        .OrderBy(d => d.Year)
        .ThenBy(d => d.Day);

    Console.WriteLine(year is null
        ? "Registered days:"
        : $"Registered days for {year}:");

    foreach (var d in days)
    {
        Console.WriteLine($"  {d.Year} Day {d.Day:00} - {d.Title}");
        Console.WriteLine($"     Variants: {string.Join(", ", d.Variants.Keys)}");
    }
}