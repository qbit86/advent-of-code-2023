using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        Debug.Assert(lines is not null);
        int rowCount = lines.Count;
        Debug.Assert(lines.Count > 0);
        int columnCount = lines[0].Length;
        UltraGraph graph = new(rowCount, columnCount);
        Point destinationPosition = new(columnCount - 1, rowCount - 1);
        return Helpers.Solve(lines, rowCount, columnCount, graph, IsDestination);

        bool IsDestination(Node node)
        {
            return node.MoveCount >= 4 && node.Position == destinationPosition;
        }
    }
}
