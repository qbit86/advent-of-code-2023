using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using Arborescence;

namespace AdventOfCode2023;

file static class GraphDrawer
{
    private static int s_nextId;

    internal static int GetNextId() => s_nextId++;
}

internal sealed class GraphDrawer<TNeighborEnumerator>
    where TNeighborEnumerator : IEnumerator<Point>
{
    private const string Indent = "  ";

    private readonly string _outputDirectory;

    private GraphDrawer(string outputDirectory)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(outputDirectory));
        _outputDirectory = outputDirectory;
    }

    internal static GraphDrawer<TNeighborEnumerator> Create(DateTime timestamp)
    {
        string outputDirectory = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Assembly.GetExecutingAssembly().GetName().Name,
            timestamp.ToString("dd_HH-mm-ss", CultureInfo.InvariantCulture));
        return new(outputDirectory);
    }

    internal void Draw<TGraph, TRows>(TGraph graph, IReadOnlyCollection<Point> vertices, MapBase<TRows> map)
        where TGraph : IOutNeighborsAdjacency<Point, TNeighborEnumerator>
        where TRows : IReadOnlyList<string>
    {
        Debug.Assert(graph is not null);
        Debug.Assert(vertices is not null);
        Debug.Assert(map is not null);
        EnsureDirectory();
        string outputFileName = $"{GraphDrawer.GetNextId()}-{vertices.Count}.gv";
        string outputPath = Path.Join(_outputDirectory, outputFileName);
        using var writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        try
        {
            writer.WriteLine($"{Indent}layout=\"neato\"");
            writer.WriteLine($"{Indent}node [shape=point]");
            foreach (var vertex in vertices)
            {
                char tile = map.GetTile(vertex);
                writer.WriteLine(tile is '.'
                    ? $"{Indent}\"{vertex}\" [label=\"\" pos=\"{vertex.X},{-vertex.Y}!\"]"
                    : $"{Indent}\"{vertex}\" [label=\"{Display(tile)}\" pos=\"{vertex.X},{-vertex.Y}!\" shape=square]");
            }

            writer.WriteLine();

            foreach (var vertex in vertices)
            {
                using var outNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
                while (outNeighborEnumerator.MoveNext())
                {
                    var neighbor = outNeighborEnumerator.Current;
                    writer.WriteLine($"{Indent}\"{vertex}\" -> \"{neighbor}\"");
                }
            }
        }
        finally
        {
            writer.WriteLine("}");
        }
    }

    internal void Draw<TGraph>(
        TGraph graph, IReadOnlyCollection<Point> vertices, IReadOnlyDictionary<Endpoints<Point>, int> weightByEdge)
        where TGraph : IOutNeighborsAdjacency<Point, TNeighborEnumerator>
    {
        Debug.Assert(graph is not null);
        Debug.Assert(vertices is not null);
        Debug.Assert(weightByEdge is not null);
        EnsureDirectory();
        string outputFileName = $"{GraphDrawer.GetNextId()}-{vertices.Count}.gv";
        string outputPath = Path.Join(_outputDirectory, outputFileName);
        using var writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        try
        {
            writer.WriteLine($"{Indent}layout=\"neato\"");
            writer.WriteLine($"{Indent}node [shape=point]");
            foreach (var vertex in vertices)
            {
                using var outNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
                bool hasMoreThanTwoNeighbors = outNeighborEnumerator.MoveNext() &&
                    outNeighborEnumerator.MoveNext() && outNeighborEnumerator.MoveNext();
                writer.WriteLine(hasMoreThanTwoNeighbors
                    ? $"{Indent}\"{vertex}\" [label=\"\" pos=\"{vertex.X},{-vertex.Y}!\" shape=square]"
                    : $"{Indent}\"{vertex}\" [label=\"\" pos=\"{vertex.X},{-vertex.Y}!\"]");
            }

            writer.WriteLine();

            foreach (var vertex in vertices)
            {
                using var outNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
                while (outNeighborEnumerator.MoveNext())
                {
                    var neighbor = outNeighborEnumerator.Current;
                    var edge = Endpoints.Create(vertex, neighbor);
                    int weight = weightByEdge.GetValueOrDefault(edge, 1);
                    writer.WriteLine($"{Indent}\"{vertex}\" -> \"{neighbor}\" [label={weight}]");
                }
            }
        }
        finally
        {
            writer.WriteLine("}");
        }
    }

    internal void Draw<TGraph>(TGraph graph, IReadOnlyCollection<Point> vertices)
        where TGraph : IOutNeighborsAdjacency<Point, TNeighborEnumerator>
    {
        Debug.Assert(graph is not null);
        Debug.Assert(vertices is not null);
        EnsureDirectory();
        string outputFileName = $"{GraphDrawer.GetNextId()}-{vertices.Count}.gv";
        string outputPath = Path.Join(_outputDirectory, outputFileName);
        using var writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        try
        {
            writer.WriteLine($"{Indent}layout=\"neato\"");
            writer.WriteLine($"{Indent}node [shape=point]");
            foreach (var vertex in vertices)
            {
                using var outNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
                bool hasMoreThanTwoNeighbors = outNeighborEnumerator.MoveNext() &&
                    outNeighborEnumerator.MoveNext() && outNeighborEnumerator.MoveNext();
                writer.WriteLine(hasMoreThanTwoNeighbors
                    ? $"{Indent}\"{vertex}\" [label=\"\" pos=\"{vertex.X},{-vertex.Y}!\" shape=square]"
                    : $"{Indent}\"{vertex}\" [label=\"\" pos=\"{vertex.X},{-vertex.Y}!\"]");
            }

            writer.WriteLine();

            foreach (var vertex in vertices)
            {
                using var outNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
                while (outNeighborEnumerator.MoveNext())
                {
                    var neighbor = outNeighborEnumerator.Current;
                    writer.WriteLine($"{Indent}\"{vertex}\" -> \"{neighbor}\"");
                }
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

    private static string Display(char tile) => tile switch
    {
        '>' => "\u2192",
        'v' => "\u2193",
        '<' => "\u2190",
        '^' => "\u2191",
        _ => tile.ToString()
    };
}
