using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SightKeeper.UI.WPF.Converters;

public sealed class EmptyToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType != typeof(Visibility))
			throw new ArgumentException($"Expected {typeof(Visibility).FullName}, but actual {targetType.FullName}", nameof(targetType));
		if (value is not IEnumerable<object> enumerable) throw new ArgumentException($"Expected enumerable, but actually {value.GetType().FullName}");
		return enumerable.Any() ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
