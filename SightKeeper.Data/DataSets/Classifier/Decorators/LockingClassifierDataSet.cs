using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier.Decorators;

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

	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new LockingTagsLibrary<StorableTag>(inner.TagsLibrary, editingLock);

	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } =
		new LockingAssetsLibrary<StorableClassifierAsset>(inner.AssetsLibrary, editingLock);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);
}