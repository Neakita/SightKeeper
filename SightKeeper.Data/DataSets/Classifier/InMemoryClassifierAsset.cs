using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class InMemoryClassifierAsset : StorableClassifierAsset
{
	public StorableImage Image { get; }
	public AssetUsage Usage { get; set; }
	public Tag Tag { get; set; }

	public InMemoryClassifierAsset(StorableImage image, Tag tag)
	{
		Image = image;
		Tag = tag;
	}
}