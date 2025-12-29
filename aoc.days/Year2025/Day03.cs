using System.Net.Http.Headers;
using System.Numerics;
using Aoc.Days;

namespace Aoc.Days.Year2025;

public sealed class Day03 : IAocDay
{
    public int Year => 2025;
    public int Day => 3;
    public string Title => "Battery Finder";

    public IReadOnlyDictionary<string, Func<string, string>> Variants { get; }

    public Day03()
    {
        Variants = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["1"] = RunPart1,
            ["2"] = RunPart2,
        };
    }

    private string RunPart1(string input)
    {
        List<string> banks = new List<string>();
        List<Battery> Batteries = new List<Battery>();
        int totalPower = 0;

        banks.AddRange(input.ToString().Split("\n"));

        foreach (var bank in banks)
        {
            if (!string.IsNullOrWhiteSpace(bank))
            {

                int pos = 0;
                foreach (var battery in bank)
                {
                    Batteries.Add(new Battery
                    {
                        value = int.Parse(battery.ToString()),
                        position = pos++
                    }
                    );
                }
                Battery temp = Batteries[0];

                //get first largest battery. If there are multiple largest batteries, get the leftmost one.
                for (int i = 0; i < Batteries.Count; i++)
                {
                    if (Batteries[i].value > temp.value && (i != Batteries.Count - 1))
                    {
                        temp = Batteries[i];
                    }
                }
                Battery firstLargest = temp;
                temp = Batteries[firstLargest.position + 1];

                //get the next largest battery to the right of the first largest battery
                for (int j = temp.position + 1; j < Batteries.Count; j++)
                {
                    if (Batteries[j].value > temp.value)
                    {
                        temp = Batteries[j];
                    }
                }

                int power = firstLargest.value.ToString() + temp.value.ToString() is string s
                    ? int.Parse(s)
                    : 0;
                totalPower += power;
                Batteries.Clear();
            }
        }

        return $"The answer is: {totalPower}";
    }

    private string RunPart2(string input)
    {
        int bank_size = 12;
        List<string> banks = new List<string>();
        List<Battery> allBatteries = new List<Battery>();
        List<Battery> selectedBatteries = new List<Battery>();
        Int64 totalPower = 0;

        banks.AddRange(input.ToString().Split("\n"));

        foreach (var bank in banks)
        {
            if (!string.IsNullOrWhiteSpace(bank))
            {

                int pos = 0;
                //store all batteries in a list with their position
                foreach (var battery in bank)
                {
                    allBatteries.Add(new Battery
                    {
                        value = int.Parse(battery.ToString()),
                        position = pos++
                    }
                    );
                }

                pos = 0;
                //get the largest possible battery in the bank
                for (int i = 0; i < bank_size; i++)
                {
                    selectedBatteries.Add(GetNextBiggestBattery(allBatteries, pos, bank_size - (i + 1)));
                    pos = selectedBatteries[i].position + 1;
                }

                string powerString = "";
                Int64 power = 0;
                //convert the selected batteries to a string and then to an int
                foreach (var battery in selectedBatteries)
                {
                    powerString += battery.value.ToString();
                }
                power = Int64.Parse(powerString);

                totalPower += power;
                allBatteries.Clear();
                selectedBatteries.Clear();
            }
        }

        return $"The answer is: {totalPower}";
    }

    private Battery GetNextBiggestBattery(List<Battery> allBatteries, int startindex = 0, int slotsToFill=12)
    {
        Battery temp =  allBatteries[startindex];
        for (int i = startindex; i < allBatteries.Count; i++)
        {
            if (allBatteries[i].value > temp.value && (i < allBatteries.Count - slotsToFill))
            {
                temp = allBatteries[i];
            }
        }

        return temp;
    }
}
public class Battery
{
    public int value { get; set; }
    public int position { get; set; }
}