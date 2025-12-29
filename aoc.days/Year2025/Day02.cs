using System.Numerics;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day02 : IAocDay
{
    public int Year => 2025;
    public int Day => 2;
    public string Title => "Invalid Product IDs";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day02()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        string[] lines = input.Split(',');
        Int64 range_start = 0;
        Int64 range_end = 0;
        List<Int64> invalids = new List<Int64>();
        //int invalids_sum = 0;

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                range_start = Int64.Parse(line.Split('-')[0]);;
                range_end = Int64.Parse(line.Split('-')[1]);

                for (Int64 i = range_start; i <= range_end; i++)
                {
                    string prod_id = i.ToString();
                    int length = prod_id.Length;
                    
                    if(prod_id.Substring(0, length/2) == 
                       prod_id.Substring(length/2, length - length/2)) 
                    {
                        invalids.Add(i);
                    }
                }
            }
        }

        //invalids_sum = invalids.Sum();
        return "The answer is: " + invalids.Sum();
    }

    private string RunPart2(string input)
    {
        string[] lines = input.Split(',');
        Int64 range_start = 0;
        Int64 range_end = 0;
        List<Int64> invalids = new List<Int64>();
        List<int> divisors = new List<int>()    ;
        //int invalids_sum = 0;

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                range_start = Int64.Parse(line.Split('-')[0]);;
                range_end = Int64.Parse(line.Split('-')[1]);

                for (Int64 i = range_start; i <= range_end; i++)
                {
                    string prod_id = i.ToString();
                    int length = prod_id.Length;

                    // get all ways prod_id is evenly divisible
                    for(int split = 1; split <= length/2; split++)
                    {
                        if(length % split == 0) 
                        {
                            divisors.Add(split);
                        }
                    }
                    
                    foreach(int div in divisors)
                    {
                        var segments = Helper.SplitByLength(prod_id, div).ToList();


                            if(segments.Distinct().Count() <= 1)
                            {
                                invalids.Add(i);
                                break;
                            }

                    }
                    divisors.Clear();
                }
            }
        }

        //invalids_sum = invalids.Sum();
        return "The answer is: " + invalids.Sum();
    }


}