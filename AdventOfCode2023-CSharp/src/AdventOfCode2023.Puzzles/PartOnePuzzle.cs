using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle : Puzzle
{
    private PartOnePuzzle() { }

    public static PartOnePuzzle Instance { get; } = new();

    protected override Hand ParseHand(StringSegment stringSegment) => PartOneHandFactory.Instance.Parse(stringSegment);
}
