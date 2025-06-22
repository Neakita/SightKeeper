using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

public sealed class StorableClassifierAssetFactory : AssetFactory<ClassifierAsset>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public ClassifierAsset CreateAsset(Image image)
	{
		Guard.IsNotNull(TagsOwner);
		return new InMemoryClassifierAsset(image, TagsOwner.Tags[0]);
	}
}