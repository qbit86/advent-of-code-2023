using System;
using System.Collections;
using System.Collections.Generic;
using Arborescence;

namespace AdventOfCode2023;

internal sealed partial class HeatLossMap
{
    public IEnumerable<Endpoints<Node>> Keys => throw new NotSupportedException();
    public IEnumerable<int> Values => throw new NotSupportedException();
    public int Count => throw new NotSupportedException();
    public IEnumerator<KeyValuePair<Endpoints<Node>, int>> GetEnumerator() => throw new NotSupportedException();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
