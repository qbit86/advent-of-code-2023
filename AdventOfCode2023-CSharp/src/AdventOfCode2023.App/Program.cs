using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode2023;

internal static class Program
{
    internal static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("puzzle/1", async () =>
        {
            var stopwatch = Stopwatch.StartNew();
            long answer = await PartOnePuzzle.SolveAsync("input.txt").ConfigureAwait(false);
            return new PuzzleSolution { Answer = answer, Duration = stopwatch.Elapsed };
        });

        app.MapGet("puzzle/2", async () =>
        {
            var stopwatch = Stopwatch.StartNew();
            long answer = await PartTwoPuzzle.SolveAsync("input.txt").ConfigureAwait(false);
            return new PuzzleSolution { Answer = answer, Duration = stopwatch.Elapsed };
        });

        app.Run();
    }
}
