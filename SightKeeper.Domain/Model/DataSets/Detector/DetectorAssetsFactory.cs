using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Detector;

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