using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Poser2D;

internal sealed class Poser2DAssetsFactory : AssetsFactory<Poser2DAsset>
{
	public Poser2DAssetsFactory(TagsContainer<DomainTag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public Poser2DAsset CreateAsset(Image image)
	{
		return new Poser2DAsset(_tagsOwner)
		{
			Image = image
		};
	}

	private readonly TagsContainer<DomainTag> _tagsOwner;
}