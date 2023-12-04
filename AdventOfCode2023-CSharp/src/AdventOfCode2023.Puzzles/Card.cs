using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

internal sealed class Card
{
    private static readonly char[] s_separators = { ' ' };

    private Card(IEnumerable<int> winningNumbers, IEnumerable<int> actualNumbers)
    {
        WinningNumbers = winningNumbers;
        ActualNumbers = actualNumbers;
    }

    internal IEnumerable<int> ActualNumbers { get; }
    internal IEnumerable<int> WinningNumbers { get; }

    internal static Card Parse(string line)
    {
        int indexOfColon = line.IndexOf(':', 0);
        if (indexOfColon < 0)
            throw new InvalidOperationException($"{nameof(indexOfColon)} < 0");
        int indexOfBar = line.IndexOf('|', indexOfColon + 1);
        if (indexOfBar < 0)
            throw new InvalidOperationException($"{nameof(indexOfBar)} < 0");
        StringSegment winningSegment = new(line, indexOfColon + 1, indexOfBar - indexOfColon - 2);
        StringTokenizer winningTokenizer = new(winningSegment, s_separators);
        IEnumerable<int> winningNumbers = winningTokenizer.Where(segment => segment.Length > 0)
            .Select(segment => int.Parse(segment, CultureInfo.InvariantCulture));
        StringSegment actualSegment = new(line, indexOfBar + 1, line.Length - indexOfBar - 1);
        StringTokenizer actualTokenizer = new(actualSegment, s_separators);
        IEnumerable<int> actualNumbers = actualTokenizer.Where(segment => segment.Length > 0)
            .Select(segment => int.Parse(segment, CultureInfo.InvariantCulture));
        return new(winningNumbers, actualNumbers);
    }
}
