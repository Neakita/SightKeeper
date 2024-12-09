using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Assets;

internal sealed class DetectorAssetViewModel : AssetViewModel<DetectorAsset>, AssetViewModelFactory<DetectorAssetViewModel, DetectorAsset>
{
	public static DetectorAssetViewModel Create(DetectorAsset value)
	{
		return new DetectorAssetViewModel(value);
	}

	public DetectorAssetViewModel(DetectorAsset value) : base(value)
	{
	}
}