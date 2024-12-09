using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerViewModel<TAssetViewModel, TAsset> : DrawerViewModel<TAssetViewModel>
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>;