using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

internal sealed class InMemoryClassifierAsset : ClassifierAsset
{
	public ManagedImage Image { get; }
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public Tag Tag { get; set; }
	public ClassifierAsset Innermost => this;

	public InMemoryClassifierAsset(ManagedImage image, Tag tag)
	{
		Image = image;
		Tag = tag;
	}
}