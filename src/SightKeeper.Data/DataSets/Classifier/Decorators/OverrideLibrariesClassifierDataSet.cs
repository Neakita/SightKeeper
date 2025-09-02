using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier.Decorators;

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

	public StorableTagsOwner<StorableTag> TagsLibrary { get; init; } = inner.TagsLibrary;
	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public StorableWeightsLibrary WeightsLibrary { get; init; } = inner.WeightsLibrary;

	public StorableClassifierDataSet Innermost => inner.Innermost;
}