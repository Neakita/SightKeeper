using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModel : ViewModel, DataSetDataContext
{
	public DataSet<Asset> Value { get; }
	public string Name => Value.Name;

	public DataSetViewModel(DataSet<Asset> value)
	{
		Value = value;
	}
}