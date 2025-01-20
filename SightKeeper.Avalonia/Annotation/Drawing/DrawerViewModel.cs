using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public abstract partial class DrawerViewModel : ViewModel
{
	[ObservableProperty] public partial Screenshot? Screenshot { get; set; }
	public abstract IReadOnlyCollection<DrawerItemViewModel> Items { get; }
	public abstract Tag? Tag { get; }

	[RelayCommand]
	protected abstract void CreateItem(Bounding bounding);
}