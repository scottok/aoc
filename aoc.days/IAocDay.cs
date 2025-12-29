public interface IAocDay
{
    int Year { get; }
    int Day { get; }
    string Title { get; }

    IReadOnlyDictionary<string, Func<string, string>> Variants { get; }
}