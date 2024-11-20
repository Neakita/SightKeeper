using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal interface AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAssetViewModel : AssetViewModel<TAsset>
	where TAsset : Asset
{
	static abstract TAssetViewModel Create(TAsset value);
}