using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class InMemoryClassifierAsset : StorableClassifierAsset
{
	public StorableImage Image { get; }
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public StorableTag Tag { get; set; }
	public StorableClassifierAsset Innermost => this;

	public InMemoryClassifierAsset(StorableImage image, StorableTag tag)
	{
		Image = image;
		Tag = tag;
	}
}