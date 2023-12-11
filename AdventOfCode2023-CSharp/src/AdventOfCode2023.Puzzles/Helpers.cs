using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023;

internal static class Helpers
{
    internal static bool PopulateNeighborsByVertex<TRows, TNeighborsByVertex>(
        TRows rows, TNeighborsByVertex neighborsByVertex, out Point startingPosition)
        where TNeighborsByVertex : IDictionary<Point, List<Point>>
        where TRows : IReadOnlyList<string>
    {
        Point? startingPositionOrDefault = default;
        for (int rowIndex = 0; rowIndex < rows.Count; ++rowIndex)
        {
            string row = rows[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                Point position = new(rowIndex, columnIndex);
                char tile = row[columnIndex];
                if (tile is 'S')
                {
                    startingPositionOrDefault = position;
                    continue;
                }

                if (tile is '|' or '\u2502')
                {
                    neighborsByVertex[position] =
                        [position with { Row = rowIndex - 1 }, position with { Row = rowIndex + 1 }];
                }
                else if (tile is '-' or '\u2500')
                {
                    neighborsByVertex[position] =
                        [position with { Column = columnIndex - 1 }, position with { Column = columnIndex + 1 }];
                }
                else if (tile is 'L' or '\u2514')
                {
                    neighborsByVertex[position] =
                        [position with { Row = rowIndex - 1 }, position with { Column = columnIndex + 1 }];
                }
                else if (tile is 'J' or '\u2518')
                {
                    neighborsByVertex[position] =
                        [position with { Row = rowIndex - 1 }, position with { Column = columnIndex - 1 }];
                }
                else if (tile is '7' or '\u2510')
                {
                    neighborsByVertex[position] =
                        [position with { Row = rowIndex + 1 }, position with { Column = columnIndex - 1 }];
                }
                else if (tile is 'F' or '\u250c')
                {
                    neighborsByVertex[position] =
                        [position with { Row = rowIndex + 1 }, position with { Column = columnIndex + 1 }];
                }
            }
        }

        startingPosition = startingPositionOrDefault.GetValueOrDefault();
        if (!startingPositionOrDefault.HasValue)
            return false;

        List<Point> startingPositionActualNeighbors = new(2);
        List<Point> startingPositionCandidateNeighbors =
        [
            startingPosition with { Row = startingPosition.Row - 1 },
            startingPosition with { Column = startingPosition.Column + 1 },
            startingPosition with { Row = startingPosition.Row + 1 },
            startingPosition with { Column = startingPosition.Column - 1 }
        ];
        foreach (Point candidateNeighbor in startingPositionCandidateNeighbors)
        {
            if (neighborsByVertex.TryGetValue(candidateNeighbor, out List<Point>? candidateNeighbors) &&
                candidateNeighbors.Any(startingPosition.Equals))
            {
                startingPositionActualNeighbors.Add(candidateNeighbor);
            }
        }

        neighborsByVertex[startingPosition] = startingPositionActualNeighbors;

        return true;
    }
}
