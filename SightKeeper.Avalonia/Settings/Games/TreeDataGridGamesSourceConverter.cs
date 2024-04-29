using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed class TreeDataGridGamesSourceConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
			return null;
		var games = (IEnumerable<GameViewModel>)value;
		return new FlatTreeDataGridSource<GameViewModel>(games)
		{
			Columns =
			{
				new TemplateColumn<GameViewModel>("Title", "GameTitleCellTemplate", width: new GridLength(1, GridUnitType.Star)),
				new TemplateColumn<GameViewModel>("Actions", "GameActionsCellTemplate", width: new GridLength(0, GridUnitType.Auto))
			}
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}