using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;

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
				new TemplateColumn<GameViewModel>(
					"Title",
					"GameTitleCellTemplate",
					width: GridLength.Star,
					options: new TemplateColumnOptions<GameViewModel>
					{
						MinWidth = new GridLength(100)
					}),
				new TemplateColumn<GameViewModel>(
					"Actions",
					"ExistingGameActionsCellTemplate",
					width: GridLength.Auto)
			}
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}