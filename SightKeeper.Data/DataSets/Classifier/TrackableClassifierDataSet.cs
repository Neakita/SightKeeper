using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

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

	public AssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<StorableClassifierAsset>(inner.AssetsLibrary, listener);

	public WeightsLibrary WeightsLibrary { get; } =
		new TrackableWeightsLibrary(inner.WeightsLibrary, listener);
}