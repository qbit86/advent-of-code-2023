using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 54L)]
    [InlineData("input.txt", 495607L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }
}
