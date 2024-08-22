using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal sealed class TreeDataGridTagsSourceConverter : FuncValueConverter<IReadOnlyCollection<TagViewModel>, FlatTreeDataGridSource<TagViewModel>>
{
	public TreeDataGridTagsSourceConverter() : base(Convert)
	{
	}

	private static FlatTreeDataGridSource<TagViewModel> Convert(IReadOnlyCollection<TagViewModel>? source)
	{
		Guard.IsNotNull(source);
		return new FlatTreeDataGridSource<TagViewModel>(source)
		{
			Columns =
			{
				new TextColumn<TagViewModel, string>(
					"Name",
					tag => tag.Name,
					SetTagName,
					new GridLength(1, GridUnitType.Star))
			}
		};
	}

	private static void SetTagName(TagViewModel tag, string? name)
	{
		Guard.IsNotNull(name);
		tag.Name = name;
	}
}