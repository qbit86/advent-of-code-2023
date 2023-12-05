using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PuzzlePartOne_Tests
{
    [Theory]
    [InlineData("sample.txt", 35L)]
    [InlineData("input.txt", 346433842L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PuzzlePartOne.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
