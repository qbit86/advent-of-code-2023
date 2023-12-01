using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static partial class PuzzlePartTwo
{
    private const string Pattern = @"(\d|eight|five|four|nine|one|seven|six|three|two)";

    private static readonly SearchValues<char> s_digits = SearchValues.Create("0123456789");

    private static Regex? s_leftToRightRegex;
    private static Regex? s_rightToLeftRegex;

    private static Regex LeftToRightRegex => s_leftToRightRegex ??= CreateLeftToRightRegex();

    private static Regex RightToLeftRegex => s_rightToLeftRegex ??= CreateRightToLeftRegex();

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IEnumerable<string> lines)
    {
        IEnumerable<(string, string)> splits = lines.Select(line => (ReplaceLeftmost(line), ReplaceRightmost(line)));
        IEnumerable<long> calibrationValues = splits.Select(Decode);
        return calibrationValues.Sum();
    }

    [GeneratedRegex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreateLeftToRightRegex();

    [GeneratedRegex(Pattern, RegexOptions.Compiled | RegexOptions.RightToLeft | RegexOptions.CultureInvariant)]
    private static partial Regex CreateRightToLeftRegex();

    private static string ReplaceLeftmost(string line) => LeftToRightRegex.Replace(line, ReplaceWithDigit, 1);

    private static string ReplaceRightmost(string line) => RightToLeftRegex.Replace(line, ReplaceWithDigit, 1);

    private static string ReplaceWithDigit(Match match) => match.Value switch
    {
        "one" => "1",
        "two" => "2",
        "three" => "3",
        "four" => "4",
        "five" => "5",
        "six" => "6",
        "seven" => "7",
        "eight" => "8",
        "nine" => "9",
        var other => other
    };

    private static long Decode((string, string) pair)
    {
        (string left, string right) = pair;
        return Decode(left, right);
    }

    private static long Decode(string left, string right)
    {
        int firstIndex = left.AsSpan().IndexOfAny(s_digits);
        int leftDigit = left[firstIndex] - '0';
        int lastIndex = right.AsSpan().LastIndexOfAny(s_digits);
        int rightDigit = right[lastIndex] - '0';
        return leftDigit * 10 + rightDigit;
    }
}
