using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal abstract class AssetViewModel : ViewModel
{
	public abstract Asset Value { get; }
}