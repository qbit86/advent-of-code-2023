using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EuclideanSpace;

namespace AdventOfCode2023;

using P2 = Point2<int>;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        Debug.Assert(lines is not null);
        int rowCount = lines.Count;
        Debug.Assert(lines.Count > 0);
        int columnCount = lines[0].Length;
        Graph graph = new(rowCount, columnCount);
        P2 destinationPosition = new(columnCount - 1, rowCount - 1);
        return Helpers.Solve(lines, rowCount, columnCount, graph, IsDestination);

        bool IsDestination(Node node)
        {
            return node.Position == destinationPosition;
        }
    }
}
