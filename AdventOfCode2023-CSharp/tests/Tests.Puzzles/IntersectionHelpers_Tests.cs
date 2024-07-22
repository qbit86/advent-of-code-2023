using AdventOfCode.Numerics;
using Xunit;

namespace AdventOfCode2023;

public sealed class IntersectionHelpers_Tests
{
    public static TheoryData<Ray2<double>, Ray2<double>> CollinearTheoryData { get; } =
        CreateCollinearTheoryData();

    public static TheoryData<Ray2<double>, Ray2<double>> IntersectionInPastTheoryData { get; } =
        CreateIntersectionInPastTheoryData();

    public static TheoryData<Ray2<double>, Ray2<double>, V2<double>> IntersectionInFutureTheoryData { get; } =
        CreateIntersectionInFutureTheoryData();

    [Theory]
    [MemberData(nameof(CollinearTheoryData), MemberType = typeof(IntersectionHelpers_Tests))]
    internal void TryGetIntersection_WhenCollinear_ReturnsFalse(
        Ray2<double> left, Ray2<double> right)
    {
        bool result = IntersectionHelpers.TryGetIntersection(left, right, out _);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IntersectionInPastTheoryData), MemberType = typeof(IntersectionHelpers_Tests))]
    internal void TryGetIntersection_WhenIntersectionInPast_ReturnsFalse(
        Ray2<double> left, Ray2<double> right)
    {
        bool result = IntersectionHelpers.TryGetIntersection(left, right, out _);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IntersectionInFutureTheoryData), MemberType = typeof(IntersectionHelpers_Tests))]
    internal void TryGetIntersection_WhenIntersectionInFuture_ReturnsTrue(
        Ray2<double> left, Ray2<double> right, V2<double> expected)
    {
        bool result = IntersectionHelpers.TryGetIntersection(left, right, out V2<double> actual);
        Assert.True(result);
        Assert.Equal(expected, actual, TolerantComparer.Default<double>());
    }

    [Fact]
    internal void GetIntersection()
    {
        var left = Ray3.Create<double>(-2, 1, 11, 4, 3, 0);
        var right = Ray3.Create<double>(3, 10, 13, 1, -1, 0);
        double timeLeft = IntersectionHelpers.GetIntersectionTime(right, left);
        double timeRight = IntersectionHelpers.GetIntersectionTime(left, right);
        Assert.Equal(3.0, timeLeft, TolerantComparer.Default<double>());
        Assert.Equal(2.0, timeRight, TolerantComparer.Default<double>());
    }

    private static TheoryData<Ray2<double>, Ray2<double>> CreateCollinearTheoryData() =>
        new()
        {
            { Ray2.Create<double>(18, 19, -1, -1), Ray2.Create<double>(20, 25, -2, -2) }
        };

    private static TheoryData<Ray2<double>, Ray2<double>> CreateIntersectionInPastTheoryData() =>
        new()
        {
            { Ray2.Create<double>(19, 13, -2, 1), Ray2.Create<double>(20, 19, 1, -5) },
            { Ray2.Create<double>(18, 19, -1, -1), Ray2.Create<double>(20, 19, 1, -5) },
            { Ray2.Create<double>(20, 25, -2, -2), Ray2.Create<double>(20, 19, 1, -5) },
            { Ray2.Create<double>(12, 31, -1, -2), Ray2.Create<double>(20, 19, 1, -5) }
        };

    private static TheoryData<Ray2<double>, Ray2<double>, V2<double>> CreateIntersectionInFutureTheoryData() =>
        new()
        {
            {
                Ray2.Create<double>(19, 13, -2, 1), Ray2.Create<double>(18, 19, -1, -1),
                V2.Create(14.0 + 1.0 / 3.0, 15.0 + 1.0 / 3.0)
            },
            {
                Ray2.Create<double>(19, 13, -2, 1), Ray2.Create<double>(20, 25, -2, -2),
                V2.Create(11.0 + 2.0 / 3.0, 16.0 + 2.0 / 3.0)
            }
        };
}
