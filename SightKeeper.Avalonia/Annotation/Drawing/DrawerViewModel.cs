using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public abstract partial class DrawerViewModel : ViewModel
{
	public abstract IReadOnlyCollection<DrawerItemViewModel> Items { get; }
	public abstract Tag? Tag { get; }

	[RelayCommand]
	protected abstract void CreateItem(Bounding bounding);
}