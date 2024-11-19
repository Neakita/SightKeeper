using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Application;

public abstract class ClassifierAnnotator
{
	public virtual void SetTag(ClassifierAsset asset, ClassifierTag tag)
	{
		asset.Tag = tag;
	}
}