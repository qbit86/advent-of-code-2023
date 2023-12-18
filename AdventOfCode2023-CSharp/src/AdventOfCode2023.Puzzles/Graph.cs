using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Arborescence;
using Arborescence.Models;

namespace AdventOfCode2023;

internal sealed class Graph :
    IOutNeighborsAdjacency<Node, IEnumerator<Node>>,
    IForwardIncidence<Node, Endpoints<Node>, IncidenceEnumerator<Node, IEnumerator<Node>>>
{
    internal Graph(int rowCount, int columnCount)
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
        IncidenceEnumeratorFactory.Create(vertex, EnumerateOutNeighbors(vertex));

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        (Point position, Size direction, int moveCount) = vertex;
        if (direction == Size.Empty)
        {
            {
                Size neighborCandidateDirection = new(0, 1);
                Point neighborCandidatePosition = position + neighborCandidateDirection;
                yield return new(neighborCandidatePosition, neighborCandidateDirection, moveCount + 1);
            }
            {
                Size neighborCandidateDirection = new(1, 0);
                Point neighborCandidatePosition = position + neighborCandidateDirection;
                yield return new(neighborCandidatePosition, neighborCandidateDirection, moveCount + 1);
            }
            yield break;
        }

        if (moveCount < 3)
        {
            Point neighborCandidatePosition = position + direction;
            if (IsWithinBounds(neighborCandidatePosition))
                yield return new(neighborCandidatePosition, direction, moveCount + 1);
        }

        {
            Size neighborCandidateDirection = direction.RotateRight();
            Point neighborCandidatePosition = position + neighborCandidateDirection;
            if (IsWithinBounds(neighborCandidatePosition))
                yield return new(neighborCandidatePosition, neighborCandidateDirection, 1);
        }
        {
            Size neighborCandidateDirection = direction.RotateLeft();
            Point neighborCandidatePosition = position + neighborCandidateDirection;
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
