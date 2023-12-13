using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023;

public sealed class PartTwoPuzzle_Tests
{
    [Theory]
    [InlineData("sample.txt", 525152L)]
    [InlineData("input.txt", 6555315065024L)]
    internal async Task SolveAsync(string inputPath, long expected)
    {
        long actual = await PartTwoPuzzle.SolveAsync(inputPath).ConfigureAwait(true);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("???.### 1,1,3", 1L)]
    [InlineData(".??..??...?##. 1,1,3", 16384L)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1L)]
    [InlineData("????.#...#... 4,1,1", 16L)]
    [InlineData("????.######..#####. 1,6,5", 2500L)]
    [InlineData("?###???????? 3,2,1", 506250L)]
    internal void Solve(string line, long expected)
    {
        long actual = PartTwoPuzzle.Solve(line);
        Assert.Equal(expected, actual);
    }
}
