using System;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface SelectedItemProvider
{
	IObservable<DetectorItem?> SelectedItemChanged { get; }
}