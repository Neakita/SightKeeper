using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModel : ViewModel
{
	public DataSet Value { get; }
	public string Name => Value.Name;
	public string Description => Value.Description;

	public DataSetViewModel(DataSet value)
	{
		Value = value;
	}
}