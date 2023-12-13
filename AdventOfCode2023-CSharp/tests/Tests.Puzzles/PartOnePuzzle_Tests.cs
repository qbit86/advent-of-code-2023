using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartOnePuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 21L)]
    [InlineData("input.txt", 6958L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartOnePuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("???.### 1,1,3", 1L)]
    [InlineData(".??..??...?##. 1,1,3", 4L)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1L)]
    [InlineData("????.#...#... 4,1,1", 1L)]
    [InlineData("????.######..#####. 1,6,5", 4L)]
    [InlineData("?###???????? 3,2,1", 10L)]
    [InlineData("..#?????.. 1,1,1", 3L)]
    internal void Solve(string line, long expected)
    {
        long actual = PartOnePuzzle.Solve(line);
        Assert.Equal(expected, actual);
    }
}
