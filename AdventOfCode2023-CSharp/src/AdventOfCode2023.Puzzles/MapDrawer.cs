using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdventOfCode2023;

internal sealed class MapDrawer
{
    private static MapDrawer? s_instance;

    private readonly string _outputDirectory;

    private MapDrawer(string outputDirectory) => _outputDirectory = outputDirectory;

    internal static MapDrawer Instance => s_instance ??= new(Helpers.OutputDirectory);

    internal void Draw(IEnumerable<string> lines, IReadOnlyCollection<Point> tiles)
    {
        var rows = lines.Select(line => line.ToCharArray()).ToList();
        Draw(rows, tiles);
    }

    private void Draw(IReadOnlyList<char[]> rows, IReadOnlyCollection<Point> tiles)
    {
        foreach (var tile in tiles)
            rows[tile.Y][tile.X] = 'O';

        string fileId = Helpers.GetNextId().ToString(CultureInfo.InvariantCulture);
        string outputFileName = $"{fileId}-{tiles.Count}.txt";
        string outputPath = Path.Join(EnsureDirectory(), outputFileName);
        using var writer = File.CreateText(outputPath);
        foreach (char[] row in rows)
            writer.WriteLine(new string(row));
    }

    private string EnsureDirectory()
    {
        if (!Directory.Exists(_outputDirectory))
            Directory.CreateDirectory(_outputDirectory);
        return _outputDirectory;
    }
}
