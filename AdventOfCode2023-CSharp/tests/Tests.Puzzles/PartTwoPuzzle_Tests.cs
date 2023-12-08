using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample-3.txt", 6L)]
#if false
    [InlineData("sample-4.txt", 3L)]
#endif
    [InlineData("input.txt", 10241191004509L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
