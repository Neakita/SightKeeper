using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class TrackableClassifierDataSet(StorableClassifierDataSet inner, ChangeListener listener) : StorableClassifierDataSet
{
	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			listener.SetDataChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			inner.Description = value;
			listener.SetDataChanged();
		}
	}

	public TagsOwner<Tag> TagsLibrary { get; } =
		new TrackableTagsLibrary<Tag>(inner.TagsLibrary, listener);

	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<StorableClassifierAsset>(inner.AssetsLibrary, listener);

	public StorableWeightsLibrary WeightsLibrary { get; } =
		new TrackableWeightsLibrary(inner.WeightsLibrary, listener);
}