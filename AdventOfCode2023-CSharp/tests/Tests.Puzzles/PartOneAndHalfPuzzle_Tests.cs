using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOneAndHalfPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 19114L)]
    [InlineData("sample-1.txt", 7540L)]
    [InlineData("sample-2.txt", 0L)]
    [InlineData("sample-3.txt", 4623L)]
    [InlineData("sample-4.txt", 0L)]
    [InlineData("sample-5.txt", 6951L)]
    [InlineData("input.txt", 397134L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOneAndHalfPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
