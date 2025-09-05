using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModel : ViewModel, DataSetDataContext
{
	public DataSet<Tag, Asset> Value { get; }
	public string Name => Value.Name;

	public DataSetViewModel(DataSet<Tag, Asset> value)
	{
		Value = value;
	}
}