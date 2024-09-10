using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ClassifierDataSetConverter : DataSetConverter
{
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PackableClassifierDataSet CreatePackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ushort? maxScreenshotsWithoutAsset,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableAsset> assets,
		ImmutableArray<PackableWeights> weights)
	{
		return new PackableClassifierDataSet(
			name,
			description,
			gameId,
			composition,
			maxScreenshotsWithoutAsset,
			screenshots,
			tags,
			assets.CastArray<PackableClassifierAsset>(),
			weights.CastArray<PackablePlainWeights>());
	}

	protected override ImmutableArray<PackableAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, ConversionSession session)
	{
		var convertedAssets = assets.Cast<ClassifierAsset>().Select(ConvertAsset).ToImmutableArray();
		return ImmutableArray<PackableAsset>.CastUp(convertedAssets);
		PackableClassifierAsset ConvertAsset(ClassifierAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			session.TagsIds[asset.Tag]);
	}
}