using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public sealed partial class DataSetsTab : ReactiveUserControl<DataSetsViewModel>
{
	public DataSetsTab()
	{
		InitializeComponent();
	}
}