public static class DayRegistry
{
    public static IReadOnlyDictionary<(int Year, int Day), IAocDay> Days { get; }
        = new IAocDay[]
        {
            new Aoc.Days.Year2025.Day01(),
            new Aoc.Days.Year2025.Day02(),
            new Aoc.Days.Year2025.Day03(),
            new Aoc.Days.Year2025.Day04(),
            new Aoc.Days.Year2025.Day05(),
            new Aoc.Days.Year2025.Day06(),
            new Aoc.Days.Year2025.Day07(),
            new Aoc.Days.Year2025.Day08(),
            new Aoc.Days.Year2025.Day09(),
        }
        .ToDictionary(d => (d.Year, d.Day));
}