using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PuzzlePartTwo_Tests
{
    [Theory]
    [InlineData("sample.txt", 30L)]
    [InlineData("input.txt", 9997537L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PuzzlePartTwo.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
