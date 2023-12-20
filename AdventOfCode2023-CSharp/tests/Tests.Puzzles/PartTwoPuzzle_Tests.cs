using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 167409079868000L)]
    [InlineData("input.txt", 127517902575337L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
