using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerViewModel<TAssetViewModel, TAsset> : DrawerViewModel
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset;