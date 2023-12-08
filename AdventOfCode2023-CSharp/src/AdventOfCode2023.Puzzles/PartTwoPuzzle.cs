using Microsoft.Extensions.Primitives;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle : Puzzle
{
    private PartTwoPuzzle() { }

    public static PartTwoPuzzle Instance { get; } = new();

    protected override Hand ParseHand(StringSegment stringSegment) => PartTwoHandFactory.Instance.Parse(stringSegment);
}
