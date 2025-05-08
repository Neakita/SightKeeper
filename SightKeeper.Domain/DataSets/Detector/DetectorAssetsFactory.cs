using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Detector;

internal sealed class DetectorAssetsFactory : AssetsFactory<DetectorAsset>
{
	public DetectorAssetsFactory(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public DetectorAsset CreateAsset(Image image)
	{
		return new DetectorAsset(_tagsOwner)
		{
			Image = image
		};
	}

	private readonly TagsContainer<Tag> _tagsOwner;
}