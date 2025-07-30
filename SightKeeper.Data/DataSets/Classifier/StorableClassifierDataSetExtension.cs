using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

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

	public TagsOwner<StorableTag> TagsLibrary => extendedInner.TagsLibrary;
	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary => extendedInner.AssetsLibrary;
	public StorableWeightsLibrary WeightsLibrary => extendedInner.WeightsLibrary;

	AssetsOwner<ClassifierAsset> ClassifierDataSet.AssetsLibrary => inner.AssetsLibrary;
}