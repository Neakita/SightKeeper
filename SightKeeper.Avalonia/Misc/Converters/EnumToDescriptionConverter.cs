using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class EnumToDescriptionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
            return GetEnumDescription(enumValue);
        return ThrowHelper.ThrowArgumentException<object>(nameof(value), "Value must be an Enum");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private string GetEnumDescription(Enum enumValue)
    {
        var enumType = enumValue.GetType();
        var memberInfos = enumType.GetMember(enumValue.ToString());
        var enumValueMemberInfo = memberInfos.First(m => m.DeclaringType == enumType);
        var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        var description = ((DescriptionAttribute)valueAttributes[0]).Description;
        return description;
    }
}