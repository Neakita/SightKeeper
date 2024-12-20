using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal sealed class TreeDataGridScreenshotsLibrariesConverter : FuncValueConverter<IReadOnlyCollection<ScreenshotsLibraryViewModel>, ITreeDataGridSource<ScreenshotsLibraryViewModel>>
{
	public TreeDataGridScreenshotsLibrariesConverter() : base(Convert)
	{
	}

	private static ITreeDataGridSource<ScreenshotsLibraryViewModel> Convert(IReadOnlyCollection<ScreenshotsLibraryViewModel>? items)
	{
		Guard.IsNotNull(items);
		FlatTreeDataGridSource<ScreenshotsLibraryViewModel> source = new(items)
		{
			Columns =
			{
				new TextColumn<ScreenshotsLibraryViewModel, string>("Name", dataSet => dataSet.Name, GridLength.Star),
				new TemplateColumn<ScreenshotsLibraryViewModel>("Actions", "ScreenshotsLibraryActions", null, GridLength.Auto)
			}
		};
		return source;
	}
}