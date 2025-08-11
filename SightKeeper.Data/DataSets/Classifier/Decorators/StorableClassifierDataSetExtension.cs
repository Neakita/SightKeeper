using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier.Decorators;

internal sealed class StorableClassifierDataSetExtension(ClassifierDataSet inner, StorableClassifierDataSet extendedInner) : StorableClassifierDataSet
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

	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new StorableTagsOwnerExtension<Tag, StorableTag>(inner.TagsLibrary, extendedInner.TagsLibrary);

	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } =
		new StorableAssetsOwnerExtension<ClassifierAsset, StorableClassifierAsset>(inner.AssetsLibrary, extendedInner.AssetsLibrary);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new StorableWeightsLibraryExtension(inner.WeightsLibrary, extendedInner.WeightsLibrary);

	public StorableClassifierDataSet Innermost => extendedInner.Innermost;
}