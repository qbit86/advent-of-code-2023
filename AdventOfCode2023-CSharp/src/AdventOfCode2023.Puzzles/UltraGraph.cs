using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Arborescence;
using Arborescence.Models;

namespace AdventOfCode2023;

internal sealed class UltraGraph :
    IOutNeighborsAdjacency<Node, IEnumerator<Node>>,
    IForwardIncidence<Node, Endpoints<Node>, IncidenceEnumerator<Node, IEnumerator<Node>>>
{
    internal UltraGraph(int rowCount, int columnCount)
    {
        Debug.Assert(rowCount > 0);
        Debug.Assert(columnCount > 0);
        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    private int RowCount { get; }
    private int ColumnCount { get; }

    public bool TryGetHead(Endpoints<Node> edge, [UnscopedRef] out Node head)
    {
        head = edge.Head;
        return IsWithinBounds(head.Position);
    }

    public IncidenceEnumerator<Node, IEnumerator<Node>> EnumerateOutEdges(Node vertex) =>
        IncidenceEnumerator<Node, IEnumerator<Node>>.Create(this, vertex);

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        (var position, var direction, int moveCount) = vertex;
        if (direction == Size.Empty)
        {
            {
                Size neighborCandidateDirection = new(0, 1);
                var neighborCandidatePosition = position + neighborCandidateDirection;
                yield return new(neighborCandidatePosition, neighborCandidateDirection, moveCount + 1);
            }
            {
                Size neighborCandidateDirection = new(1, 0);
                var neighborCandidatePosition = position + neighborCandidateDirection;
                yield return new(neighborCandidatePosition, neighborCandidateDirection, moveCount + 1);
            }
            yield break;
        }

        if (moveCount < 10)
        {
            var neighborCandidatePosition = position + direction;
            if (IsWithinBounds(neighborCandidatePosition))
                yield return new(neighborCandidatePosition, direction, moveCount + 1);
        }

        if (moveCount >= 4)
        {
            var neighborCandidateDirection = direction.RotateRight();
            var neighborCandidatePosition = position + neighborCandidateDirection;
            if (IsWithinBounds(neighborCandidatePosition))
                yield return new(neighborCandidatePosition, neighborCandidateDirection, 1);
        }

        if (moveCount >= 4)
        {
            var neighborCandidateDirection = direction.RotateLeft();
            var neighborCandidatePosition = position + neighborCandidateDirection;
            if (IsWithinBounds(neighborCandidatePosition))
                yield return new(neighborCandidatePosition, neighborCandidateDirection, 1);
        }
    }

    private bool IsWithinBounds(Point position)
    {
        if (unchecked((uint)position.Y) >= (uint)RowCount)
            return false;
        if (unchecked((uint)position.X) >= (uint)ColumnCount)
            return false;
        return true;
    }
}
