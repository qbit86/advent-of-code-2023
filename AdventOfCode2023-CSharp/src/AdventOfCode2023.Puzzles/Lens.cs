using System.Diagnostics;

namespace AdventOfCode2023;

internal readonly struct Lens
{
    private Lens(string label, int focalLength, int boxIndex)
    {
        Label = label;
        FocalLength = focalLength;
        BoxIndex = boxIndex;
    }

    internal string Label { get; }
    internal int FocalLength { get; }
    internal int BoxIndex { get; }

    internal static Lens Create(string label, int focalLength)
    {
        int boxIndex = Helpers.Hash(label);
        return CreateUnchecked(label, focalLength, boxIndex);
    }

    internal static Lens CreateUnchecked(string label, int focalLength, int boxIndex)
    {
        Debug.Assert(label is not null);
        Debug.Assert(focalLength >= 1);
        Debug.Assert(focalLength <= 9);
        return new(label, focalLength, boxIndex);
    }
}
