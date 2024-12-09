using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

internal sealed class Poser2DAssetsFactory : AssetsFactory<Poser2DAsset>
{
	public Poser2DAssetsFactory(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public override Poser2DAsset CreateAsset()
	{
		return new Poser2DAsset(_tagsOwner);
	}

	private readonly TagsOwner _tagsOwner;
}