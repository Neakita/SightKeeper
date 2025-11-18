using SightKeeper.Data.DataSets.Artifacts;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class TrackableDataSet<TTag, TAsset>(DataSet<TTag, TAsset> inner, ChangeListener listener) : DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>
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

	public TagsOwner<TTag> TagsLibrary { get; } =
		new TrackableTagsLibrary<TTag>(inner.TagsLibrary, listener);

	public AssetsOwner<TAsset> AssetsLibrary { get; } =
		new TrackableAssetsLibrary<TAsset>(inner.AssetsLibrary, listener);

	public ArtifactsLibrary ArtifactsLibrary { get; } =
		new TrackableArtifactsLibrary(inner.ArtifactsLibrary, listener);

	public DataSet<TTag, TAsset> Inner => inner;
}