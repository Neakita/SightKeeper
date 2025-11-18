using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Decorators;

internal sealed class OverrideLibrariesDataSet<TTag, TAsset>(DataSet<TTag, TAsset> inner) : DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>, IDisposable
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

	public TagsOwner<TTag> TagsLibrary { get; init; } = inner.TagsLibrary;
	public AssetsOwner<TAsset> AssetsLibrary { get; init; } = inner.AssetsLibrary;
	public ArtifactsLibrary ArtifactsLibrary { get; init; } = inner.ArtifactsLibrary;
	public DataSet<TTag, TAsset> Inner => inner;

	public void Dispose()
	{
		Dispose(TagsLibrary, AssetsLibrary, ArtifactsLibrary);
	}

	private static void Dispose(params IEnumerable<object> objects)
	{
		foreach (var disposable in objects.OfType<IDisposable>())
			disposable.Dispose();
	}
}