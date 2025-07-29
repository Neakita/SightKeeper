using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class LockingClassifierDataSet(StorableClassifierDataSet inner, Lock editingLock) : StorableClassifierDataSet
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

	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } =
		new LockingAssetsLibrary<StorableClassifierAsset>(inner.AssetsLibrary, editingLock);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);
}