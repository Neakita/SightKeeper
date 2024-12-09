using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel<TAssetViewModel, TAsset> : DataSetContextViewModel
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset
{
	public abstract override ToolBarViewModel<TAssetViewModel, TAsset> ToolBar { get; }
}