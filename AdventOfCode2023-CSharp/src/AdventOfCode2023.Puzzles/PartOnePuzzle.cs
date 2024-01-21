using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode2023;

public static class PartOnePuzzle
{
    public static async Task<long> SolveAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve<TLines>(TLines lines)
        where TLines : IReadOnlyList<string>
    {
        var moduleById = lines.Select(ParsingHelpers.ParseModule).ToDictionary(m => m.Id, m => m);
        var modules = moduleById.Values.ToList();
        foreach (Module module in modules)
        {
            foreach (string destinationId in module.DestinationIds)
            {
                if (moduleById.GetValueOrDefault(destinationId) is ConjunctionModule conjunctionModule)
                    _ = conjunctionModule.TryAddInputModule(module.Id);
            }
        }

        var channel = Channel.CreateUnbounded<Message>();
        ChannelWriter<Message> channelWriter = channel.Writer;
        ChannelReader<Message> channelReader = channel.Reader;
        int lowCount = 0;
        int highCount = 0;
        for (int i = 0; i < 1000; ++i)
        {
            _ = channelWriter.TryWrite(new("button", BroadcastModule.BroadcasterId, Pulse.Low));
            while (channelReader.TryRead(out Message message))
            {
                if (message.Pulse is Pulse.Low)
                    ++lowCount;
                else if (message.Pulse is Pulse.High)
                    ++highCount;
                else
                    throw new InvalidOperationException(message.Pulse.ToString());

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
        }

        return lowCount * (long)highCount;
    }
}
