using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawers;

internal abstract class DrawerViewModel<TAssetViewModel> : DrawerViewModel
	where TAssetViewModel : AssetViewModel;