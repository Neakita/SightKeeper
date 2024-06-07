namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
	public Tag Tag { get; set; }

	internal ClassifierAsset(Screenshot screenshot, Tag tag) : base(screenshot)
	{
		Tag = tag;
	}
}