using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
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
        var startingNodes = network.NeighborsByNode.Keys.Where(it => it.EndsWith('A')).ToList();
        Dictionary<string, int> stepCountByStartingNode = new();
        foreach (string startingNode in startingNodes)
        {
            string currentNode = startingNode;
            for (int i = 0;; ++i)
            {
                if (currentNode.EndsWith('Z'))
                {
                    stepCountByStartingNode[startingNode] = i;
                    break;
                }

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

        IEnumerable<BigInteger> stepCounts =
            stepCountByStartingNode.Values.Select(BigInteger.CreateTruncating).ToList();
        BigInteger gcd = stepCounts.Aggregate(BigInteger.GreatestCommonDivisor);
        BigInteger result = stepCounts.Aggregate(Fold);
        return long.CreateChecked(result);

        BigInteger Fold(BigInteger accumulator, BigInteger current)
        {
            return accumulator / gcd * current;
        }
    }
}
