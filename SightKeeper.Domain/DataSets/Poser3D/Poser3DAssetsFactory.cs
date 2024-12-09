using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

internal sealed class Poser3DAssetsFactory : AssetsFactory<Poser3DAsset>
{
	public Poser3DAssetsFactory(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	public override Poser3DAsset CreateAsset()
	{
		return new Poser3DAsset(_tagsOwner);
	}

	private readonly TagsOwner _tagsOwner;
}