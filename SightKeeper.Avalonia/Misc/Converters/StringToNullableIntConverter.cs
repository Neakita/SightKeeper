using System;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class StringToNullableIntConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        Guard.IsOfType<int>(value);
        return value.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        Guard.IsOfType<string>(value);
        var str = (string)value;
        return int.TryParse(str, out var result) ? result : null;
    }
}