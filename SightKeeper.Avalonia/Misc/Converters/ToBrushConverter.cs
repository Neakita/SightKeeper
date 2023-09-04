using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class ToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var opacity = parameter != null && float.TryParse(parameter.ToString(), new NumberFormatInfo { NumberDecimalSeparator = "." }, out var parsedOpacity) ? parsedOpacity : 1;
        return value switch
        {
            uint uintColor => new SolidColorBrush(Color.FromUInt32(uintColor), opacity),
            Color color => new SolidColorBrush(color, opacity),
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}