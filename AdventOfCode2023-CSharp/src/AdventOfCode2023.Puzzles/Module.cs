using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2023;

internal abstract record Module
{
    private readonly string[] _destinationIds;

    internal Module(string id, string[] destinationIds)
    {
        Id = id;
        _destinationIds = destinationIds;
    }

    internal string Id { get; }

    internal IReadOnlyList<string> DestinationIds => _destinationIds;

    internal abstract Pulse Handle(Message message);
}

internal sealed record FlipFlopModule : Module
{
    internal FlipFlopModule(string id, string[] destinationIds) : base(id, destinationIds) { }

    private bool IsOn { get; set; }

    internal override Pulse Handle(Message message)
    {
        Debug.Assert(message.Destination == Id);
        if (message.Pulse is Pulse.High)
            return Pulse.None;
        Pulse pulseToSend = IsOn ? Pulse.Low : Pulse.High;
        IsOn = !IsOn;
        return pulseToSend;
    }
}

internal sealed record ConjunctionModule : Module
{
    private readonly Dictionary<string, Pulse> _pulseByInputModule;

    private ConjunctionModule(string id, string[] destinationIds, Dictionary<string, Pulse> pulseByInputModule) :
        base(id, destinationIds) =>
        _pulseByInputModule = pulseByInputModule;

    internal IReadOnlyCollection<string> InputModules => _pulseByInputModule.Keys;

    internal IReadOnlyDictionary<string, Pulse> PulseByInputModule => _pulseByInputModule;

    internal static ConjunctionModule Create(string id, string[] destinationIds) =>
        new(id, destinationIds, new());

    internal static ConjunctionModule Create(string id, string[] destinationIds, IEnumerable<string> inputModules)
    {
        var pulseByInputModule = inputModules.ToDictionary(it => it, _ => Pulse.Low);
        return new(id, destinationIds, pulseByInputModule);
    }

    internal bool TryAddInputModule(string inputModuleId) => _pulseByInputModule.TryAdd(inputModuleId, Pulse.Low);

    internal override Pulse Handle(Message message)
    {
        Debug.Assert(message.Destination == Id);
        _pulseByInputModule[message.Source] = message.Pulse;
        return _pulseByInputModule.All(it => it.Value is Pulse.High)
            ? Pulse.Low
            : Pulse.High;
    }
}

internal sealed record BroadcastModule : Module
{
    internal const string BroadcasterId = "broadcaster";

    internal BroadcastModule(string[] destinationIds) : base(BroadcasterId, destinationIds) { }

    internal override Pulse Handle(Message message)
    {
        Debug.Assert(message.Destination == BroadcasterId);
        return message.Pulse;
    }
}
