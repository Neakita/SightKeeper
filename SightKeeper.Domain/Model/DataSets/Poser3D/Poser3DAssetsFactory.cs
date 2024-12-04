using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAssetsFactory : AssetsFactory<Poser3DAsset>
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