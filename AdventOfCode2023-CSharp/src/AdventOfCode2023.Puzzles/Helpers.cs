using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2023;

internal static class Helpers
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    private static readonly string[] s_blankLines = { "\n\n", "\r\n\r\n" };
    private static readonly string[] s_newLines = { "\n", "\r\n" };

    internal static string[] SplitByBlankLines(string text) => text.Split(s_blankLines, SplitOptions);

    internal static string[] SplitByNewLines(string text) => text.Split(s_newLines, SplitOptions);

    internal static bool TryFindHorizontalAxis<TLines>(TLines lines, out int horizontalAxisPosition)
        where TLines : IReadOnlyList<string> => TryFindHorizontalAxis(lines, -1, out horizontalAxisPosition);

    internal static bool TryFindHorizontalAxis<TLines>(TLines lines, int except, out int horizontalAxisPosition)
        where TLines : IReadOnlyList<string>
    {
        int rowCount = lines.Count;
        for (horizontalAxisPosition = 1; horizontalAxisPosition < rowCount; ++horizontalAxisPosition)
        {
            if (horizontalAxisPosition == except)
                continue;
            int count = Math.Min(horizontalAxisPosition, rowCount - horizontalAxisPosition);
            bool foundDefect = false;
            for (int relativeRowIndex = 0; relativeRowIndex < count; ++relativeRowIndex)
            {
                string row = lines[horizontalAxisPosition + relativeRowIndex];
                string antiRow = lines[horizontalAxisPosition - relativeRowIndex - 1];
                if (row != antiRow)
                {
                    foundDefect = true;
                    break;
                }
            }

            if (!foundDefect)
                return true;
        }

        return false;
    }

    internal static bool TryFindVerticalAxis<TLines>(TLines lines, out int verticalAxisPosition)
        where TLines : IReadOnlyList<string> => TryFindVerticalAxis(lines, -1, out verticalAxisPosition);


    internal static bool TryFindVerticalAxis<TLines>(TLines lines, int except, out int verticalAxisPosition)
        where TLines : IReadOnlyList<string>
    {
        Debug.Assert(lines.Count > 0);
        int columnCount = lines[0].Length;
        for (verticalAxisPosition = 1; verticalAxisPosition < columnCount; ++verticalAxisPosition)
        {
            if (verticalAxisPosition == except)
                continue;
            int columnPositionCopy = verticalAxisPosition;
            int count = Math.Min(verticalAxisPosition, columnCount - verticalAxisPosition);
            bool foundDefect = false;
            for (int relativeColumnIndex = 0; relativeColumnIndex < count; ++relativeColumnIndex)
            {
                int relativeColumnIndexCopy = relativeColumnIndex;
                IEnumerable<char> column = lines.Select(line => line[columnPositionCopy + relativeColumnIndexCopy]);
                IEnumerable<char> antiColumn =
                    lines.Select(line => line[columnPositionCopy - relativeColumnIndexCopy - 1]);
                if (!column.SequenceEqual(antiColumn))
                {
                    foundDefect = true;
                    break;
                }
            }

            if (!foundDefect)
                return true;
        }

        return false;
    }
}
