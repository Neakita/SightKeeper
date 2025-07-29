using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class OverrideLibrariesClassifierDataSet(StorableClassifierDataSet inner) : StorableClassifierDataSet
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
	public AssetsOwner<StorableClassifierAsset> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public StorableWeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;
}