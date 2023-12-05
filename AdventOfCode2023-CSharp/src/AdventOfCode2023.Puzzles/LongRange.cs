namespace AdventOfCode2023;

internal readonly record struct LongRange(long Start, long End)
{
    public override string ToString() => $"[{Start}..{End})";
}
