using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace AdventOfCode2023;

public static class Helpers
{
    private static int s_nextId;
    private static string? s_outputDirectory;

    public static string OutputDirectory => s_outputDirectory ??= CreateOutputDirectoryPath();

    public static int GetNextId() => s_nextId++;

    public static string EnsureOutputDirectory()
    {
        string outputDirectory = OutputDirectory;
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        return outputDirectory;
    }

    private static string CreateOutputDirectoryPath()
    {
        var timestamp = DateTime.Now;
        return Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Assembly.GetExecutingAssembly().GetName().Name,
            timestamp.ToString("dd_HH-mm-ss", CultureInfo.InvariantCulture));
    }
}
