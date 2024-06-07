namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
{
	public ClassifierTag Tag { get; set; }

	internal ClassifierAsset(Screenshot screenshot, ClassifierTag tag) : base(screenshot)
	{
		Tag = tag;
	}
}