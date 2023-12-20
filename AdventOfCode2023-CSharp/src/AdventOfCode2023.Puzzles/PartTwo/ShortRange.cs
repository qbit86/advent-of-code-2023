namespace AdventOfCode2023;

internal readonly record struct ShortRange(short Start, short End)
{
    internal bool IsEmpty => End <= Start;
    internal int Length => End - Start;

    public override string ToString() => $"[{Start}, {End})";

    internal bool Contains(short point) => Start <= point && point < End;
}
