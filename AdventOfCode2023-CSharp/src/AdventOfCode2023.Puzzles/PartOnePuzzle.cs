using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IEnumerable<string> lines) => lines.Select(Extrapolate).Sum();

    private static long Extrapolate(string line)
    {
        nint[] values = line.Split(' ').Select(it => nint.Parse(it, CultureInfo.InvariantCulture)).ToArray();
        return Extrapolate(values);
    }

    private static nint Extrapolate(IReadOnlyList<nint> values)
    {
        List<IReadOnlyList<nint>> diffLists = [values];
        for (int i = 0;; ++i)
        {
            IEnumerable<(nint First, nint Second)> zip = diffLists[i].Zip(diffLists[i].Skip(1));
            var diffList = zip.Select(it => it.Second - it.First).ToList();
            if (diffList.All(it => it is 0))
                break;
            diffLists.Add(diffList);
        }

        nint extrapolatedValue = 0;
        for (int i = diffLists.Count - 1; i >= 0; --i)
        {
            IReadOnlyList<nint> diffList = diffLists[i];
            extrapolatedValue = diffList[^1] + extrapolatedValue;
        }

        return extrapolatedValue;
    }
}
