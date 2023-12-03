using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PuzzlePartOne_Tests
{
    [Theory]
    [InlineData("sample.txt", 4361L)]
    [InlineData("input.txt", 512794L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PuzzlePartOne.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
