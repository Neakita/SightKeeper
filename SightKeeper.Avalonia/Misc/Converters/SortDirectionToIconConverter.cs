using System;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;
using DynamicData.Binding;
using Material.Icons;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class SortDirectionToIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(MaterialIconKind))
            ThrowHelper.ThrowArgumentException(nameof(targetType), $"Must be {typeof(MaterialIconKind)}");
        if (value is SortDirection sortDirection)
        {
            return sortDirection switch
            {
                SortDirection.Ascending => MaterialIconKind.SortAscending,
                SortDirection.Descending => MaterialIconKind.SortDescending,
                _ => ThrowHelper.ThrowArgumentOutOfRangeException<object>(nameof(value))
            };
        }
        ThrowHelper.ThrowArgumentException(nameof(value), $"Must be of type {typeof(SortDirection)}");
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}