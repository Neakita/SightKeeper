using SightKeeper.Application.Misc;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets;

internal sealed class InMemoryDataSetFactory<TTag, TAsset>(
	Func<TagFactory<TTag>> createTagFactory,
	Func<AssetFactory<TAsset>> createAssetFactory,
	Func<Wrapper<Artifact>> createArtifactWrapper)
	: Factory<InMemoryDataSet<TTag, TAsset>> where TAsset : Asset
{
	public InMemoryDataSet<TTag, TAsset> Create()
	{
		var tagFactory = createTagFactory();
		var assetFactory = createAssetFactory();
		var artifactWrapper = createArtifactWrapper();
		return new InMemoryDataSet<TTag, TAsset>(tagFactory, assetFactory, artifactWrapper);
	}
}