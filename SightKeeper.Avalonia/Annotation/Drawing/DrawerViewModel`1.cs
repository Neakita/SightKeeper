using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal abstract class DrawerViewModel<TAssetViewModel> : DrawerViewModel
	where TAssetViewModel : AssetViewModel;