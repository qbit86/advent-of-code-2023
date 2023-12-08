using System;
using System.Collections.Generic;
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

    private static long Solve(IReadOnlyList<string> lines)
    {
        string instructions = lines[0];
        var network = Network.Parse(lines.Skip(2));
        int instructionCount = instructions.Length;
        string currentNode = "AAA";
        for (int i = 0;; ++i)
        {
            if (string.Equals(currentNode, "ZZZ", StringComparison.Ordinal))
                return i;
            char direction = instructions[i % instructionCount];
            (string left, string right) = network.NeighborsByNode[currentNode];
            currentNode = direction switch
            {
                'L' => left,
                'R' => right,
                _ => throw new ArgumentException(lines[0], nameof(lines))
            };
        }
    }
}
