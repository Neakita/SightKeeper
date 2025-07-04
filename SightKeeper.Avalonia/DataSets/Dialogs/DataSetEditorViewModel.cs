using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetEditorViewModel : ViewModel, DataSetEditorDataContext
{
	[ObservableProperty] public partial string Name { get; set; } = string.Empty;
	[ObservableProperty] public partial string Description { get; set; } = string.Empty;

	public DataSetEditorViewModel()
	{
	}

	public DataSetEditorViewModel(DataSet dataSet)
	{
		Name = dataSet.Name;
		Description = dataSet.Description;
	}
}