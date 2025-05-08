using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Poser3D;

internal sealed class Poser3DAssetsFactory : AssetsFactory<Poser3DAsset>
{
	public Poser3DAssetsFactory(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public Poser3DAsset CreateAsset(Image image)
	{
		return new Poser3DAsset(_tagsOwner)
		{
			Image = image
		};
	}

	private readonly TagsContainer<Tag> _tagsOwner;
}