using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartTwoPuzzle
{
    private const bool IsDrawingEnabled = false;

    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        var destinationIdsByTypedId = lines.Select(ParsingHelpers.ParseRaw)
            .ToDictionary(kv => kv.Key.ToString(), kv => kv.Value);
        var moduleById = destinationIdsByTypedId.Select(kv => ParsingHelpers.ParseModule(kv.Key, kv.Value))
            .ToDictionary(module => module.Id, module => module);
        foreach ((string id, Module module) in moduleById)
        {
            Debug.Assert(id == module.Id);
            foreach (string destinationId in module.DestinationIds)
            {
                if (moduleById.GetValueOrDefault(destinationId) is ConjunctionModule conjunctionModule)
                    _ = conjunctionModule.TryAddInputModule(id);
            }
        }

        if (IsDrawingEnabled)
        {
            GraphDrawer.Instance.Draw(moduleById, RankDictionary.Instance);

            Dictionary<string, Module> cnXpModuleById = CreateModuleById("%cn", "&xp", destinationIdsByTypedId);
            GraphDrawer.Instance.Draw(cnXpModuleById, RankDictionary.Instance);

            Dictionary<string, Module> csGpModuleById = CreateModuleById("%cs", "&gp", destinationIdsByTypedId);
            GraphDrawer.Instance.Draw(csGpModuleById, RankDictionary.Instance);

            Dictionary<string, Module> mlLnModuleById = CreateModuleById("%ml", "&ln", destinationIdsByTypedId);
            GraphDrawer.Instance.Draw(mlLnModuleById, RankDictionary.Instance);

            Dictionary<string, Module> vlXlModuleById = CreateModuleById("%vl", "&xl", destinationIdsByTypedId);
            GraphDrawer.Instance.Draw(vlXlModuleById, RankDictionary.Instance);
        }

        string sinkParentId = destinationIdsByTypedId.Single(kv => kv.Value.Contains("rx")).Key.TrimStart('%', '&');
        var sinkParentModule = (ConjunctionModule)moduleById[sinkParentId];
        int count = sinkParentModule.InputModules.Count;
        Dictionary<string, int> buttonPressesById = new(count);
        Dictionary<string, int> highPulsesById = new(count);

        for (int buttonPresses = 1; buttonPressesById.Count < count; ++buttonPresses)
        {
            highPulsesById.Clear();
            var channel = Channel.CreateUnbounded<Message>();
            ChannelReader<Message> channelReader = channel.Reader;
            ChannelWriter<Message> channelWriter = channel.Writer;
            _ = channelWriter.TryWrite(new("button", BroadcastModule.BroadcasterId, Pulse.Low));
            while (channelReader.TryRead(out Message message))
            {
                if (message.Destination == sinkParentId && message.Pulse is Pulse.High)
                    ++CollectionsMarshal.GetValueRefOrAddDefault(highPulsesById, message.Source, out bool _);
                if (!moduleById.TryGetValue(message.Destination, out Module? module))
                    continue;
                Pulse pulseToSend = module.Handle(message);
                if (pulseToSend is Pulse.None)
                    continue;
                foreach (string destinationId in module.DestinationIds)
                {
                    Message messageToSend = new(module.Id, destinationId, pulseToSend);
                    channelWriter.TryWrite(messageToSend);
                }
            }

            foreach (KeyValuePair<string, int> kv in highPulsesById)
                _ = buttonPressesById.TryAdd(kv.Key, buttonPresses);
        }

        var buttonPressCounts = buttonPressesById.Values.Select(BigInteger.CreateTruncating).ToList();
        BigInteger gcd = buttonPressCounts.Aggregate(BigInteger.GreatestCommonDivisor);
        BigInteger lcm = buttonPressCounts.Aggregate((accumulator, current) => accumulator / gcd * current);
        return long.CreateChecked(lcm);
    }

    private static Dictionary<string, Module> CreateModuleById(
        string sourceHeadTypedId, string sinkTailTypedId, Dictionary<string, string[]> destinationIdsByTypedId)
    {
        Dictionary<string, string[]> d =
            CreateDestinationIdsByTypedId(sourceHeadTypedId, sinkTailTypedId, destinationIdsByTypedId);
        return d.Select(kv => ParsingHelpers.ParseModule(kv.Key, kv.Value))
            .ToDictionary(module => module.Id, module => module);
    }

    private static Dictionary<string, string[]> CreateDestinationIdsByTypedId(
        string sourceHeadTypedId, string sinkTailTypedId, Dictionary<string, string[]> destinationIdsByTypedId)
    {
        var result = destinationIdsByTypedId.ToDictionary();
        result[BroadcastModule.BroadcasterId] = [sourceHeadTypedId.TrimStart('%', '&')];
        ReadOnlySpan<string> sinkTailTypeIds = ["&xp", "&gp", "&ln", "&xl"];
        foreach (string typedId in sinkTailTypeIds)
        {
            if (typedId != sinkTailTypedId)
                result[typedId] = [];
        }

        return result;
    }
}
