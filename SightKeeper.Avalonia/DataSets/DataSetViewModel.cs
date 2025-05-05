using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModel : ViewModel, DataSetDataContext
{
	public DataSet Value { get; }
	public string Name => Value.Name;

	public DataSetViewModel(DataSet value)
	{
		Value = value;
	}
}