using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal abstract class AssetViewModel<TAsset> : AssetViewModel
	where TAsset : Asset
{
	public sealed override TAsset Value { get; }

	protected AssetViewModel(TAsset value)
	{
		Value = value;
	}
}