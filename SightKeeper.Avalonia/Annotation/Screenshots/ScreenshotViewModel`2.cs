using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class ScreenshotViewModel<TAssetViewModel, TAsset> : ScreenshotViewModel<TAssetViewModel>
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override Screenshot<TAsset> Value { get; }
	public override TAssetViewModel? Asset => _asset;

	public ScreenshotViewModel(ScreenshotImageLoader imageLoader, Screenshot<TAsset> value) : base(imageLoader)
	{
		Value = value;
	}

	internal void UpdateAsset()
	{
		// should I laugh or cry
		if (Asset?.Value == Value.Asset)
			return;
		OnPropertyChanging(nameof(Asset));
		_asset = Value.Asset == null ? null : TAssetViewModel.Create(Value.Asset);
		OnPropertyChanged(nameof(Asset));
	}

	private TAssetViewModel? _asset;
}