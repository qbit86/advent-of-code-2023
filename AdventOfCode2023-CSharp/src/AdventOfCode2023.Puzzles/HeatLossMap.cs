using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arborescence;
using EuclideanSpace;

namespace AdventOfCode2023;

using P2 = Point2<int>;

internal sealed partial class HeatLossMap : IReadOnlyDictionary<Endpoints<Node>, int>
{
    private readonly int[][] _rows;

    private HeatLossMap(int[][] rows)
    {
        Debug.Assert(rows is not null);
        _rows = rows;
    }

    public bool ContainsKey(Endpoints<Node> key) => ContainsKey(key.Head.Position);

    public bool TryGetValue(Endpoints<Node> key, out int value) => TryGetHeatLoss(key.Head, out value);

    public int this[Endpoints<Node> key] =>
        TryGetValue(key, out int value) ? value : throw new ArgumentOutOfRangeException(nameof(key));

    internal static HeatLossMap Create<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        Debug.Assert(lines is not null);
        int[][] rows = lines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();
        return new(rows);
    }

    private bool ContainsKey(P2 position)
    {
        if (unchecked((uint)position.Y) >= (uint)_rows.Length)
            return false;
        int[] row = _rows[position.Y];
        return unchecked((uint)position.X) < (uint)row.Length;
    }

    private bool TryGetHeatLoss(Node node, out int heatLoss) => TryGetHeatLoss(node.Position, out heatLoss);

    private bool TryGetHeatLoss(P2 position, out int heatLoss)
    {
        if (unchecked((uint)position.Y) >= (uint)_rows.Length)
            return None(out heatLoss);
        int[] row = _rows[position.Y];
        if (unchecked((uint)position.X) >= (uint)row.Length)
            return None(out heatLoss);
        heatLoss = row[position.X];
        return true;
    }

    private static bool None(out int value)
    {
        value = default;
        return false;
    }
}
