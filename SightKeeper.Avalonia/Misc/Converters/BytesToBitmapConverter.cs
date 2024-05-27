using System;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class BytesToBitmapConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not byte[] bytes || !targetType.IsAssignableFrom(typeof(Bitmap))) // converter used for the wrong type
			return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
		using MemoryStream stream = new(bytes);
		return new Bitmap(stream);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}