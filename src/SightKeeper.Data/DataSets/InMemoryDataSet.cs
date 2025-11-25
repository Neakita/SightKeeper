using SightKeeper.Data.DataSets.Artifacts;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal sealed class InMemoryDataSet<TTag, TAsset>(
	TagFactory<TTag> tagFactory,
	AssetFactory<TAsset> assetFactory,
	Wrapper<Artifact> artifactWrapper)
	: DataSet<TTag, TAsset>, PostWrappingInitializable<DataSet<TTag, TAsset>>
	where TAsset : Asset
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TagsOwner<TTag> TagsLibrary { get; } = new InMemoryTagsLibrary<TTag>(tagFactory);
	public AssetsOwner<TAsset> AssetsLibrary { get; } = new InMemoryAssetsLibrary<TAsset>(assetFactory);
	public ArtifactsLibrary ArtifactsLibrary { get; } = new InMemoryArtifactsLibrary(artifactWrapper);

	public void Initialize(DataSet<TTag, TAsset> wrapped)
	{
		foreach (var initializable in Initializables)
			initializable.Initialize(wrapped);
	}

	private IEnumerable<PostWrappingInitializable<DataSet<TTag, TAsset>>> Initializables =>
		((IEnumerable<object>)[TagsLibrary, AssetsLibrary, ArtifactsLibrary])
		.SelectMany(GetInitializables);

	private static IEnumerable<PostWrappingInitializable<DataSet<TTag, TAsset>>> GetInitializables(object obj)
	{
		return obj.Get<PostWrappingInitializable<DataSet<TTag, TAsset>>>();
	}
}