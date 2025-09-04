using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier.Decorators;

internal sealed class OverrideLibrariesClassifierDataSet(ClassifierDataSet inner) : ClassifierDataSet, Decorator<ClassifierDataSet>
{
	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public string Description
	{
		get => inner.Description;
		set => inner.Description = value;
	}

	public TagsOwner<Tag> TagsLibrary { get; init; } = inner.TagsLibrary;
	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;
	public ClassifierDataSet Inner => inner;
}