using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 405L)]
    [InlineData("sample-1.txt", 5L)]
    [InlineData("sample-2.txt", 400L)]
    [InlineData("sample-3.txt", 1L)]
    [InlineData("input.txt", 34918L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
