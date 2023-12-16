using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    private const int BoxCount = 256;

    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    private static readonly char[] s_separators = { '=', '-' };
    private static readonly Dictionary<string, byte> s_boxIndexByLensLabel = new();
    private static readonly List<Lens>[] s_lensesByBoxIndex = new List<Lens>[BoxCount];

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string line = await File.ReadAllTextAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return SolveUnchecked(line);
    }

    public static long Solve(string line)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(line);
        return SolveUnchecked(line);
    }

    private static long SolveUnchecked(string line)
    {
        string[] steps = line.Split(',', SplitOptions);
        return Solve(steps);
    }

    private static long Solve(IEnumerable<string> steps)
    {
        foreach (string step in steps)
        {
            string[] parts = step.Split(s_separators, 3, SplitOptions);
            if (parts.Length is 1)
                Remove(parts[0]);
            else if (parts.Length is 2)
                Replace(parts[0], parts[1]);
            else
                throw new ArgumentException(step, nameof(steps));
        }

        long focusingPowerSum = 0;
        for (int boxIndex = 0; boxIndex < BoxCount; ++boxIndex)
        {
            if (!TryGetLenses(byte.CreateChecked(boxIndex), out List<Lens> lenses))
                continue;
            for (int lensIndex = 0; lensIndex < lenses.Count; ++lensIndex)
            {
                Lens lens = lenses[lensIndex];
                focusingPowerSum += (1L + boxIndex) * (1L + lensIndex) * lens.FocalLength;
            }
        }

        return focusingPowerSum;
    }

    private static void Remove(string lensLabel)
    {
        byte boxIndex = GetBoxIndex(lensLabel);
        if (!TryGetLenses(boxIndex, out List<Lens> lenses))
            return;
        for (int lensIndex = lenses.Count - 1; lensIndex >= 0; lensIndex--)
        {
            if (lenses[lensIndex].Label == lensLabel)
                lenses.RemoveAt(lensIndex);
        }
    }

    private static void Replace(string lensLabel, string focalLengthString)
    {
        if (!int.TryParse(focalLengthString, out int focalLength))
            throw new ArgumentException(focalLengthString, nameof(focalLengthString));
        byte boxIndex = GetBoxIndex(lensLabel);
        List<Lens> lenses = GetOrAddLenses(boxIndex);
        var newLens = Lens.CreateUnchecked(lensLabel, focalLength, boxIndex);
        int lensIndex = lenses.FindIndex(l => l.Label == lensLabel);
        if (lensIndex < 0)
            lenses.Add(newLens);
        else
            lenses[lensIndex] = newLens;
    }

    private static byte GetBoxIndex(string lensLabel)
    {
        if (s_boxIndexByLensLabel.TryGetValue(lensLabel, out byte boxIndex))
            return boxIndex;

        boxIndex = Helpers.Hash(lensLabel);
        s_boxIndexByLensLabel.Add(lensLabel, boxIndex);
        return boxIndex;
    }

    private static List<Lens> GetOrAddLenses(byte boxIndex) => TryGetLenses(boxIndex, out List<Lens> lenses)
        ? lenses
        : s_lensesByBoxIndex[boxIndex] = new();

    private static bool TryGetLenses(byte boxIndex, out List<Lens> lenses)
    {
        lenses = s_lensesByBoxIndex[boxIndex];
        return lenses is not null;
    }
}
