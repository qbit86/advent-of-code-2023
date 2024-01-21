using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2023;

file static class Helpers
{
    private static int s_nextId;

    internal static int GetNextId() => s_nextId++;
}

internal sealed class GraphDrawer
{
    private const string Indent = "  ";

    private static GraphDrawer? s_instance;

    private readonly string _outputDirectory;

    private GraphDrawer(string outputDirectory) => _outputDirectory = outputDirectory;

    internal static GraphDrawer Instance => s_instance ??= Create(DateTime.Now);

    private static GraphDrawer Create(DateTime timestamp)
    {
        string outputDirectory = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Assembly.GetExecutingAssembly().GetName().Name,
            timestamp.ToString("dd_HH-mm-ss", CultureInfo.InvariantCulture));
        return new(outputDirectory);
    }

    internal void Draw(IReadOnlyDictionary<string, Module> moduleById, IReadOnlyDictionary<string, int> rankById)
    {
        ModuleIdComparer comparer = new(rankById);
        var definedModules = moduleById.Values.ToList();
        var moduleIds = definedModules.Select(it => it.DestinationIds).SelectMany(it => it)
            .Concat(definedModules.Select(it => it.Id)).Distinct()
            .Order(comparer).ToList();

        string fileId = Helpers.GetNextId().ToString(CultureInfo.InvariantCulture);
        string outputFileName = $"{fileId}-{moduleById.Count}.gv";
        string outputPath = Path.Join(EnsureDirectory(), outputFileName);
        using StreamWriter writer = File.CreateText(outputPath);
        writer.WriteLine($"digraph \"{outputFileName}\" {{");
        try
        {
            writer.WriteLine($"{Indent}node [fontname=\"Helvetica\", style=filled]");
            foreach (string moduleId in moduleIds)
            {
                writer.Write($"{Indent}{moduleId}");
                if (!moduleById.TryGetValue(moduleId, out Module? module))
                {
                    writer.WriteLine();
                    continue;
                }

                if (module is FlipFlopModule)
                    writer.Write(" [fillcolor=lightblue]");
                else if (module is ConjunctionModule)
                    writer.Write(" [fillcolor=darkseagreen1]");
                writer.WriteLine();
            }

            ILookup<int, string> lookup = moduleIds.ToLookup(rankById.GetValueOrDefault);
            writer.WriteLine();
            foreach (IGrouping<int, string> grouping in lookup)
            {
                string rank = grouping.Key switch
                {
                    int.MinValue => "source",
                    int.MaxValue => "sink",
                    _ => "same"
                };
                writer.WriteLine($"{Indent}{{ rank={rank}; {string.Join(' ', grouping)} }}");
            }

            foreach (Module module in moduleById.Values)
            {
                writer.WriteLine();
                foreach (string destinationId in module.DestinationIds)
                    writer.WriteLine($"{Indent}{module.Id} -> {destinationId}");
            }
        }
        finally
        {
            writer.WriteLine("}");
        }
    }

    private string EnsureDirectory()
    {
        if (!Directory.Exists(_outputDirectory))
            Directory.CreateDirectory(_outputDirectory);
        return _outputDirectory;
    }
}
