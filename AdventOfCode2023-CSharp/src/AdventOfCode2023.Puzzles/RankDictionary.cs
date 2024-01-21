using Arborescence;

namespace AdventOfCode2023;

internal sealed class RankDictionary : IPartialReadOnlyDictionary<string, int>
{
    internal static RankDictionary Instance { get; } = new();

    public bool TryGetValue(string key, out int value)
    {
        value = GetValue(key);
        return true;
    }

    private static int GetValue(string key) => key switch
    {
        BroadcastModule.BroadcasterId => int.MinValue,
        "cn" or "cs" or "ml" or "vl" => int.MinValue + 1,
        "sj" or "rg" or "pp" or "zp" => int.MinValue + 2,
        "xp" or "gp" or "ln" or "xl" => int.MaxValue - 2,
        "df" => int.MaxValue - 1,
        "rx" => int.MaxValue,
        _ => 0
    };
}
