using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Arborescence;
using Arborescence.Traversal.Adjacency;
using static System.FormattableString;

namespace AdventOfCode2023;

file static class Program
{
    private const int CellSize = 48;

    private static Task Main() => PlotMapsAsync();

    private static async Task PlotMapsAsync()
    {
        string[] paths = ["sample.txt", "input.txt"];
        foreach (string path in paths)
            await PlotMapAsync(path).ConfigureAwait(false);
    }

    private static async Task PlotMapAsync(string path)
    {
        string[] lines = await File.ReadAllLinesAsync(path, Encoding.UTF8).ConfigureAwait(false);
        var map = PartOneMap.Create(lines);
        var graph = Graph.Create(map);
        Dictionary<Point, int> distanceByTile = new() { { map.Start, 0 } };
        var edges = EnumerableBfs<Point>.EnumerateEdges(graph, map.Start).ToList();

        foreach (Endpoints<Point> edge in edges)
        {
            int tailDistance = distanceByTile[edge.Tail];
            int headDistance = tailDistance + 1;
            distanceByTile.Add(edge.Head, headDistance);
        }

        int height = lines.Length;
        int width = lines[0].Length;
        XNamespace ns = "http://www.w3.org/2000/svg";
        XElement root = new(ns + "svg",
            new XAttribute("version", "2"),
            new XAttribute("width", width * CellSize),
            new XAttribute("height", height * CellSize));
        XDocument doc = new(root);

        XElement mazeContainer = new(ns + "g");
        root.Add(mazeContainer);
        mazeContainer.Add(new XElement(ns + "rect",
            new XAttribute("fill", "lightgrey"),
            new XAttribute("x", 0),
            new XAttribute("y", 0),
            new XAttribute("width", width * CellSize),
            new XAttribute("height", height * CellSize)));
        for (int rowIndex = 0; rowIndex < height; ++rowIndex)
        {
            string line = lines[rowIndex];
            for (int columnIndex = 0; columnIndex < line.Length; ++columnIndex)
            {
                if (map.IsWalkable(rowIndex, columnIndex))
                    continue;

                const string color = "black";
                mazeContainer.Add(new XElement(ns + "rect",
                    new XAttribute("fill", color),
                    new XAttribute("x", columnIndex * CellSize),
                    new XAttribute("y", rowIndex * CellSize),
                    new XAttribute("width", CellSize),
                    new XAttribute("height", CellSize)));
            }
        }

        const int margin = 8;
        XElement pathNodeContainer = new(ns + "g");
        root.Add(pathNodeContainer);
        int upperBoundInclusive = (width + height) / 2;
        for (int rowIndex = 0; rowIndex < height; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < width; ++columnIndex)
            {
                if (!map.IsWalkable(rowIndex, columnIndex))
                    continue;

                string color = ComputeColor(new(columnIndex, rowIndex));
                pathNodeContainer.Add(new XElement(ns + "rect",
                    new XAttribute("fill", color),
                    new XAttribute("x", columnIndex * CellSize + margin),
                    new XAttribute("y", rowIndex * CellSize + margin),
                    new XAttribute("width", CellSize - 2 * margin),
                    new XAttribute("height", CellSize - 2 * margin)));
            }
        }

        XElement pathEdgeContainer = new(ns + "g");
        root.Add(pathEdgeContainer);
        foreach (Endpoints<Point> edge in edges)
        {
            string tailColor = ComputeColor(edge.Tail);
            string headColor = ComputeColor(edge.Head);
            string color = $"color-mix(in hsl, {tailColor}, {headColor})";
            if (edge.Head.Y == edge.Tail.Y)
            {
                int left = Math.Min(edge.Tail.X, edge.Head.X);
                int x = (left + 1) * CellSize - margin;
                int y = edge.Tail.Y * CellSize + margin;
                const int w = 2 * margin;
                const int h = CellSize - 2 * margin;
                pathEdgeContainer.Add(new XElement(ns + "rect",
                    new XAttribute("fill", color),
                    new XAttribute("x", x),
                    new XAttribute("y", y),
                    new XAttribute("width", w),
                    new XAttribute("height", h)));
            }
            else if (edge.Head.X == edge.Tail.X)
            {
                int x = edge.Tail.X * CellSize + margin;
                int upper = Math.Min(edge.Tail.Y, edge.Head.Y);
                int y = (upper + 1) * CellSize - margin;
                const int w = CellSize - 2 * margin;
                const int h = 2 * margin;
                pathEdgeContainer.Add(new XElement(ns + "rect",
                    new XAttribute("fill", color),
                    new XAttribute("x", x),
                    new XAttribute("y", y),
                    new XAttribute("width", w),
                    new XAttribute("height", h)));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        string fileId = Helpers.GetNextId().ToString(CultureInfo.InvariantCulture);
        string outputFileName = $"{fileId}-{lines.Length}.svg";
        string outputPath = Path.Join(Helpers.EnsureOutputDirectory(), outputFileName);

        doc.Save(outputPath);
        return;

        string ComputeColor(Point vertex)
        {
            bool isReachable = distanceByTile.TryGetValue(new(vertex.X, vertex.Y), out int distance);
            if (!isReachable)
                return "red";

            if (distance > upperBoundInclusive)
                return "darkorange";

            double fraction = distance / (double)upperBoundInclusive;
            double amount = fraction * fraction;
            double lightness = double.Lerp(25, 75, amount);
            return Invariant($"hsl(120, 100%, {lightness:F}%)");
        }
    }
}
