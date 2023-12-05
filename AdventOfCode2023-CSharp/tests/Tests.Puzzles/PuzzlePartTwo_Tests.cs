using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PuzzlePartTwo_Tests
{
    [Theory]
    [InlineData("sample.txt", 46L)]
    [InlineData("input.txt", 60294664L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PuzzlePartTwo.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
