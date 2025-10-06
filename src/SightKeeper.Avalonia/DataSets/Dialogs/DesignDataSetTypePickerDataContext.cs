using System.Collections.Generic;
using System.Linq;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DesignDataSetTypePickerDataContext : DataSetTypePickerDataContext
{
	public IReadOnlyCollection<DataSetTypeDataContext> Types { get; init; }
	public DataSetTypeDataContext SelectedType { get; set; }

	public DesignDataSetTypePickerDataContext(params IEnumerable<string> typeNames)
	{
		Types = typeNames
			.Select(name => new DesignDataSetTypeDataContext(name))
			.ToList();
		SelectedType = Types.First();
	}
}