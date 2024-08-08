using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    public static Task<long> SolveAsync(string path) => SolveAsync(path, 64);

    public static async Task<long> SolveAsync(string path, int stepCount)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines, stepCount);
    }

    private static long Solve<TLines>(TLines lines, int stepCount)
        where TLines : IReadOnlyList<string>
    {
        var map = PartOneMap.CreateUnchecked(lines);
        var graph = Graph.Create(map);
        Dictionary<Point, int> distanceByTile = new() { { map.Start, 0 } };
        var edges = EnumerableBfs<Point>.EnumerateEdges(graph, map.Start);
        var tileDistancePairs = edges.Select(edge =>
            {
                int tailDistance = distanceByTile[edge.Tail];
                int headDistance = tailDistance + 1;
                distanceByTile.Add(edge.Head, headDistance);
                return KeyValuePair.Create(edge.Head, headDistance);
            }
        ).Prepend(KeyValuePair.Create(map.Start, 0));

        int startParity = (map.Start.X + map.Start.Y) & 1;
        int desiredParity = (stepCount & 1) ^ startParity;
        var filteredNeighborhood = tileDistancePairs
            .TakeWhile(kv => kv.Value <= stepCount)
            .Select(kv => kv.Key)
            .Where(CheckParity);
        return filteredNeighborhood.Count();

        bool CheckParity(Point point)
        {
            return ((point.X + point.Y) & 1) == desiredParity;
        }
    }
}
