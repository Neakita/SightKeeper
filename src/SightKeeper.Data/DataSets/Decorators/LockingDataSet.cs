using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class LockingDataSet<TTag, TAsset>(DataSet<TTag, TAsset> inner, Lock editingLock) : DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>
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

	public TagsOwner<TTag> TagsLibrary { get; } =
		new LockingTagsLibrary<TTag>(inner.TagsLibrary, editingLock);

	public AssetsOwner<TAsset> AssetsLibrary { get; } =
		new LockingAssetsLibrary<TAsset>(inner.AssetsLibrary, editingLock);

	public WeightsLibrary WeightsLibrary { get; } =
		new LockingWeightsLibrary(inner.WeightsLibrary, editingLock);

	public DataSet<TTag, TAsset> Inner => inner;
}