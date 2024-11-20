using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract partial class DrawerViewModel : ViewModel
{
	public abstract IReadOnlyCollection<DrawerItemViewModel> Items { get; }

	[RelayCommand]
	protected abstract void CreateItem(Bounding bounding);
}