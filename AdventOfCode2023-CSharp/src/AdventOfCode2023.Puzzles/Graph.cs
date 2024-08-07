using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Arborescence;

namespace AdventOfCode2023;

internal static class Graph
{
    internal static Graph<TRows> Create<TRows>(TRows rows) where TRows : IReadOnlyList<string> =>
        Graph<TRows>.Create(rows);
}

internal sealed class Graph<TRows> : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
    where TRows : IReadOnlyList<string>
{
    private readonly TRows _rows;

    private Graph(TRows rows, int rowCount)
    {
        Debug.Assert(rows is not null);
        Debug.Assert(rowCount == rows.Count);
        RowCount = rowCount;
        _rows = rows;
    }

    private int RowCount { get; }

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        if (!TryGetTile(vertex.Position, out char vertexTile))
            yield break;

        switch (vertexTile)
        {
            case '.':
            {
                var neighborCandidatePosition = vertex.Position + vertex.Direction;
                if (IsWithinGrid(neighborCandidatePosition))
                    yield return vertex with { Position = neighborCandidatePosition };
                yield break;
            }
            case '/':
            {
                var neighborCandidateDirection = Directions.GetNewDirectionForForwardMirror(vertex.Direction);
                var neighborCandidatePosition = vertex.Position + neighborCandidateDirection;
                if (IsWithinGrid(neighborCandidatePosition))
                    yield return new(neighborCandidatePosition, neighborCandidateDirection);
                yield break;
            }
            case '\\':
            {
                var neighborCandidateDirection = Directions.GetNewDirectionForBackwardMirror(vertex.Direction);
                var neighborCandidatePosition = vertex.Position + neighborCandidateDirection;
                if (IsWithinGrid(neighborCandidatePosition))
                    yield return new(neighborCandidatePosition, neighborCandidateDirection);
                yield break;
            }
            case '|':
                if (Directions.IsHorizontal(vertex.Direction))
                {
                    var topNeighborCandidatePosition = vertex.Position + Directions.Up;
                    if (IsWithinGrid(topNeighborCandidatePosition))
                        yield return new(topNeighborCandidatePosition, Directions.Up);
                    var bottomNeighborCandidatePosition = vertex.Position + Directions.Down;
                    if (IsWithinGrid(bottomNeighborCandidatePosition))
                        yield return new(bottomNeighborCandidatePosition, Directions.Down);
                }
                else if (Directions.IsVertical(vertex.Direction))
                {
                    var neighborCandidatePosition = vertex.Position + vertex.Direction;
                    if (IsWithinGrid(neighborCandidatePosition))
                        yield return vertex with { Position = neighborCandidatePosition };
                }

                yield break;
            case '-':
                if (Directions.IsHorizontal(vertex.Direction))
                {
                    var neighborCandidatePosition = vertex.Position + vertex.Direction;
                    if (IsWithinGrid(neighborCandidatePosition))
                        yield return vertex with { Position = neighborCandidatePosition };
                }
                else if (Directions.IsVertical(vertex.Direction))
                {
                    var leftNeighborCandidatePosition = vertex.Position + Directions.Left;
                    if (IsWithinGrid(leftNeighborCandidatePosition))
                        yield return new(leftNeighborCandidatePosition, Directions.Left);
                    var rightNeighborCandidatePosition = vertex.Position + Directions.Right;
                    if (IsWithinGrid(rightNeighborCandidatePosition))
                        yield return new(rightNeighborCandidatePosition, Directions.Right);
                }

                yield break;
            default:
                throw new InvalidOperationException(vertexTile.ToString());
        }
    }

    internal static Graph<TRows> Create(TRows rows)
    {
        Debug.Assert(rows is not null);
        return new(rows, rows.Count);
    }

    private bool IsWithinGrid(Point position)
    {
        if (unchecked((uint)position.Y >= (uint)RowCount))
            return false;

        string row = _rows[position.Y];
        if (unchecked((uint)position.X >= (uint)row.Length))
            return false;

        return true;
    }

    private bool TryGetTile(Point position, out char tile)
    {
        if (unchecked((uint)position.Y >= (uint)RowCount))
            return None(out tile);

        string row = _rows[position.Y];
        if (unchecked((uint)position.X >= (uint)row.Length))
            return None(out tile);

        tile = row[position.X];
        return true;
    }

    private static bool None(out char tile)
    {
        tile = default;
        return false;
    }
}
