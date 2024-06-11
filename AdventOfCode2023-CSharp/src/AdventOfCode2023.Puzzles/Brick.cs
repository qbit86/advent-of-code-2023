using System;
using System.Globalization;

namespace AdventOfCode2023;

internal sealed class Brick : IEquatable<Brick>
{
    private readonly string _representation;

    private Brick(int id, string representation,
        int lowerBoundX, int lowerBoundY, int lowerBoundZ,
        int upperBoundXInclusive, int upperBoundYInclusive, int lengthZInclusive)
    {
        Id = id;
        _representation = representation;
        LowerBoundX = lowerBoundX;
        LowerBoundY = lowerBoundY;
        LowerBoundZ = lowerBoundZ;
        UpperBoundXInclusive = upperBoundXInclusive;
        UpperBoundYInclusive = upperBoundYInclusive;
        LengthZInclusive = lengthZInclusive;
    }

    internal int Id { get; }

    internal int LowerBoundX { get; }

    internal int LowerBoundY { get; }

    internal int LowerBoundZ { get; set; }

    internal int UpperBoundXInclusive { get; }

    internal int UpperBoundYInclusive { get; }

    internal int LengthZInclusive { get; }

    internal int UpperBoundZInclusive => LowerBoundZ + LengthZInclusive;

    public bool Equals(Brick? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override string ToString() => _representation;

    internal static Brick Parse(int id, ReadOnlySpan<char> line)
    {
        Span<Range> ranges = stackalloc Range[7];
        int count = line.SplitAny(ranges, "~,", StringSplitOptions.TrimEntries);
        if (count is not 6)
            throw new ArgumentException(null, nameof(line));
        int lowerBoundX = P(line[ranges[0]]);
        int lowerBoundY = P(line[ranges[1]]);
        int lowerBoundZ = P(line[ranges[2]]);
        int upperBoundXInclusive = P(line[ranges[3]]);
        int upperBoundYInclusive = P(line[ranges[4]]);
        int upperBoundZInclusive = P(line[ranges[5]]);
        if (lowerBoundX > upperBoundXInclusive || lowerBoundY > upperBoundYInclusive ||
            lowerBoundZ > upperBoundZInclusive)
            throw new ArgumentException(null, nameof(line));
        if (lowerBoundZ > upperBoundZInclusive)
            throw new ArgumentException(null, nameof(line));
        int lengthZInclusive = upperBoundZInclusive - lowerBoundZ;
        Span<char> buffer = stackalloc char[13 + line.Length];
        if (!id.TryFormat(buffer, out int charsWritten, "N0", CultureInfo.InvariantCulture))
            throw new InvalidOperationException();
        buffer[charsWritten] = ':';
        buffer[charsWritten + 1] = ' ';
        line.CopyTo(buffer[(charsWritten + 2)..]);
        string representation = buffer[..(charsWritten + 2 + line.Length)].ToString();
        return new Brick(id, representation,
            lowerBoundX, lowerBoundY, lowerBoundZ, upperBoundXInclusive, upperBoundYInclusive, lengthZInclusive);

        static int P(ReadOnlySpan<char> s)
        {
            return int.Parse(s, CultureInfo.InvariantCulture);
        }
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Brick other && Equals(other);

    public override int GetHashCode() => Id;

    public static bool operator ==(Brick? left, Brick? right) => Equals(left, right);

    public static bool operator !=(Brick? left, Brick? right) => !Equals(left, right);
}
