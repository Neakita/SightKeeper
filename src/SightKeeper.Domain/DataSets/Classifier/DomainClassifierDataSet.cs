using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class DomainClassifierDataSet : ClassifierDataSet, Decorator<ClassifierDataSet>
{
	public string Name
	{
		get => Inner.Name;
		set => Inner.Name = value;
	}

	public string Description
	{
		get => Inner.Description;
		set => Inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; }
	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public ClassifierDataSet Inner { get; }

	public DomainClassifierDataSet(ClassifierDataSet inner)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<Tag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary, 2);
	}
}