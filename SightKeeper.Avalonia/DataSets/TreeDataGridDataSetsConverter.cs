using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class TreeDataGridDataSetsConverter : FuncValueConverter<IReadOnlyCollection<DataSetViewModel>, ITreeDataGridSource<DataSetViewModel>>
{
	public TreeDataGridDataSetsConverter() : base(Convert)
	{
	}

	private static ITreeDataGridSource<DataSetViewModel> Convert(IReadOnlyCollection<DataSetViewModel>? items)
	{
		Guard.IsNotNull(items);
		FlatTreeDataGridSource<DataSetViewModel> source = new(items)
		{
			Columns =
			{
				new TextColumn<DataSetViewModel, string>("Name", dataSet => dataSet.Name)
			}
		};
		return source;
	}
}