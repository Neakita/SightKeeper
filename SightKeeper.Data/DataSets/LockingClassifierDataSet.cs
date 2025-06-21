using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Model.DataSets.Tags;
using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Model.DataSets;

internal sealed class LockingClassifierDataSet(ClassifierDataSet inner, Lock editingLock) : ClassifierDataSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			lock (editingLock)
				inner.Name = value;
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			lock (editingLock)
				inner.Description = value;
		}
	}

	public TagsOwner<Tag> TagsLibrary { get; } =
		new LockingTagsLibrary<Tag>(inner.TagsLibrary, editingLock);

	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; } =
		new LockingAssetsLibrary<ClassifierAsset>(inner.AssetsLibrary, editingLock);

	public WeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);
}