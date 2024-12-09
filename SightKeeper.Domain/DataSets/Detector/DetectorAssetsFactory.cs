using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

internal sealed class DetectorAssetsFactory : AssetsFactory<DetectorAsset>
{
	public DetectorAssetsFactory(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public override DetectorAsset CreateAsset()
	{
		return new DetectorAsset(_tagsOwner);
	}

	private readonly TagsOwner _tagsOwner;
}