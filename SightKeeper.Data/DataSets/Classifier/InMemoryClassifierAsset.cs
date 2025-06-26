using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class InMemoryClassifierAsset : ClassifierAsset
{
	public Image Image { get; }
	public AssetUsage Usage { get; set; }
	public Tag Tag { get; set; }

	public InMemoryClassifierAsset(Image image, Tag tag)
	{
		Image = image;
		Tag = tag;
	}
}