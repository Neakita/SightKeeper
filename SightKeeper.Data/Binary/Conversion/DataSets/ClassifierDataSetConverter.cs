using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ClassifierDataSetConverter : PlainDataSetConverter<PackableClassifierAsset, PackableClassifierDataSet>
{
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags)
	{
		return ConvertPlainTags(tags);
	}

	protected override ImmutableArray<PackableClassifierAsset> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<ClassifierAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableClassifierAsset ConvertAsset(ClassifierAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			Session.TagsIds[asset.Tag]);
	}

	protected override ImmutableArray<PackablePlainWeights> ConvertWeights(IReadOnlyCollection<Weights> weights)
	{
		return ConvertPlainWeights(weights);
	}
}