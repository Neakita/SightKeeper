using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class DomainClassifierDataSet : ClassifierDataSet
{
	public string Name
	{
		get => _inner.Name;
		set => _inner.Name = value;
	}

	public string Description
	{
		get => _inner.Description;
		set => _inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; }
	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }

	public DomainClassifierDataSet(ClassifierDataSet inner)
	{
		_inner = inner;
		TagsLibrary = new DomainTagsLibrary<Tag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary, 2);
	}

	private readonly ClassifierDataSet _inner;
}