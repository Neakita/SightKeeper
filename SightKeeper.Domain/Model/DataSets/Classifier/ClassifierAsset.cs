namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
	public Tag Tag { get; set; }

	public ClassifierAsset(Screenshot screenshot, Tag tag) : base(screenshot)
	{
		Tag = tag;
	}
}