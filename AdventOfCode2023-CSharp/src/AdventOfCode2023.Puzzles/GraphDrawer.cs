using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Edge = Arborescence.Endpoints<string>;

namespace AdventOfCode2023;

file static class Helpers
{
    private static int s_nextId;

    internal static int GetNextId() => s_nextId++;
}

internal sealed class GraphDrawer
{
    private const string Indent = "  ";

    private readonly string _outputDirectory;

    private GraphDrawer(string outputDirectory)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(outputDirectory));
        _outputDirectory = outputDirectory;
    }

    internal static GraphDrawer Create() => Create(DateTime.Now);

    private static GraphDrawer Create(DateTime timestamp)
    {
        string outputDirectory = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Assembly.GetExecutingAssembly().GetName().Name,
            timestamp.ToString("dd_HH-mm-ss", CultureInfo.InvariantCulture));
        return new(outputDirectory);
    }

    internal void Draw<TNeighborDictionary, TVertices>(
        TNeighborDictionary neighborsByComponent, TVertices vertices)
        where TNeighborDictionary : IReadOnlyDictionary<string, List<string>>
        where TVertices : IReadOnlyCollection<string>
    {
        Debug.Assert(neighborsByComponent is not null);
        EnsureDirectory();
        string id = Helpers.GetNextId().ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
        string outputFileName = $"{id}-{neighborsByComponent.Count}.gv";
        string outputPath = Path.Join(_outputDirectory, outputFileName);
        using StreamWriter writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        try
        {
            writer.WriteLine($"{Indent}node [fontname=\"Helvetica\"]");
            bool needsNewLineSeparator = false;
            foreach (string vertex in vertices)
                writer.WriteLine($"{Indent}{vertex}");

            writer.WriteLine();
            foreach (string vertex in vertices)
            {
                if (needsNewLineSeparator)
                    writer.WriteLine();
                if (!neighborsByComponent.TryGetValue(vertex, out List<string>? neighbors))
                    continue;
                foreach (string neighbor in neighbors)
                    writer.WriteLine($"{Indent}{vertex} -> {neighbor}");
                needsNewLineSeparator = true;
            }
        }
        finally
        {
            writer.WriteLine("}");
        }
    }

    internal void Draw(IncidenceAdjacency ia)
    {
        Debug.Assert(ia is not null);
        EnsureDirectory();
        int vertexCount = ia.Vertices.Count;
        IReadOnlyList<Edge> edges = ia.Edges;
        int edgeCount = edges.Count;
        int width = int.CreateTruncating(Math.Log10(edgeCount)) + 1;
        string id = Helpers.GetNextId().ToString(CultureInfo.InvariantCulture).PadLeft(width, '0');
        string outputFileName = $"{id}-{vertexCount}-{edgeCount}.gv";
        string outputPath = Path.Join(_outputDirectory, outputFileName);
        using StreamWriter writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        IReadOnlyDictionary<Edge, string> tailByEdge = ia.TailByEdge;
        IReadOnlyDictionary<Edge, string> headByEdge = ia.HeadByEdge;
        try
        {
            writer.WriteLine($"{Indent}node [fontname=\"Helvetica\"]");
            writer.WriteLine($"{Indent}edge [fontname=\"Times-Italic\"]");
            foreach (Edge edge in edges)
            {
                string tail = tailByEdge[edge];
                string head = headByEdge[edge];
                writer.WriteLine(edgeCount <= 4
                    ? $"{Indent}{tail} -> {head} [label=\"{edge}\"]"
                    : $"{Indent}{tail} -> {head}");
            }
        }
        finally
        {
            writer.WriteLine("}");
        }
    }

    private void EnsureDirectory()
    {
        if (!Directory.Exists(_outputDirectory))
            Directory.CreateDirectory(_outputDirectory);
    }
}
