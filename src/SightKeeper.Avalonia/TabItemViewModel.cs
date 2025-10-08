using System;
using Material.Icons;

namespace SightKeeper.Avalonia;

public sealed class TabItemViewModel(MaterialIconKind iconKind, string header, Func<(object, IDisposable)> contentFactory) : ViewModel
{
	public MaterialIconKind IconKind => iconKind;
	public string Header => header;
	public Func<(object, IDisposable)> ContentFactory => contentFactory;
}