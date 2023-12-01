using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PuzzlePartTwo_Tests
{
    [Theory]
    [InlineData("sample-2.txt", 281L)]
    [InlineData("sample-3.txt", 23L)]
    [InlineData("input.txt", 54087L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PuzzlePartTwo.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
