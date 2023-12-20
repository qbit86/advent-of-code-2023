using System;
using System.Diagnostics;

namespace AdventOfCode2023;

internal abstract class Rule
{
    protected Rule(ReadOnlyMemory<char> rawText, ReadOnlyMemory<char> destination)
    {
        Debug.Assert(!rawText.IsEmpty);
        Debug.Assert(!destination.IsEmpty);
        RawText = rawText;
        Destination = destination;
    }

    private ReadOnlyMemory<char> RawText { get; }
    internal ReadOnlyMemory<char> Destination { get; }

    public override string ToString() => new(RawText.Span);
}

internal sealed class TransitionRule : Rule
{
    internal TransitionRule(ReadOnlyMemory<char> rawText, ReadOnlyMemory<char> destination) :
        base(rawText, destination) { }

    internal bool IsAccepted => Destination.Span is "A";
    internal bool IsRejected => Destination.Span is "R";
}

internal abstract class ComparisonRule : Rule
{
    protected ComparisonRule(ReadOnlyMemory<char> rawText, ReadOnlyMemory<char> destination,
        int categoryIndex, short operand) : base(rawText, destination)
    {
        Debug.Assert((uint)categoryIndex < 4u);
        CategoryIndex = categoryIndex;
        Operand = operand;
    }

    internal short Operand { get; }
    internal int CategoryIndex { get; }

    internal abstract bool Satisfies(Part part);
}

internal sealed class LessComparisonRule : ComparisonRule
{
    internal LessComparisonRule(ReadOnlyMemory<char> rawText, ReadOnlyMemory<char> destination,
        int categoryIndex, short operand)
        : base(rawText, destination, categoryIndex, operand) { }

    internal override bool Satisfies(Part part) => part.GetValue(CategoryIndex) < Operand;
}

internal sealed class GreaterComparisonRule : ComparisonRule
{
    internal GreaterComparisonRule(ReadOnlyMemory<char> rawText, ReadOnlyMemory<char> destination,
        int categoryIndex, short operand)
        : base(rawText, destination, categoryIndex, operand) { }

    internal override bool Satisfies(Part part) => part.GetValue(CategoryIndex) > Operand;
}
