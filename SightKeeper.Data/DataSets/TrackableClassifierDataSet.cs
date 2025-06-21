using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets;

internal sealed class TrackableClassifierDataSet(ClassifierDataSet inner, ChangeListener listener) : ClassifierDataSet
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

	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<ClassifierAsset>(inner.AssetsLibrary, listener);

	public WeightsLibrary WeightsLibrary { get; } =
		new TrackableWeightsLibrary(inner.WeightsLibrary, listener);
}