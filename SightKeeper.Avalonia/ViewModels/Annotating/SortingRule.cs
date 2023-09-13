using System;
using System.Collections.Generic;
using DynamicData.Binding;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SortingRule<T>
{
    public string Name { get; }
    public SortDirection Direction { get; }
    public IComparer<T> Comparer { get; }

    public SortingRule(string name, SortDirection direction, Func<T, IComparable> expression)
    {
        Name = name;
        Direction = direction;
        Comparer = direction == SortDirection.Ascending
            ? SortExpressionComparer<T>.Ascending(expression)
            : SortExpressionComparer<T>.Descending(expression);
    }

    public override string ToString() => Name;
}