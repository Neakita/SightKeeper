using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class ClassifierDataSetConverter : DataSetConverter<PackableClassifierDataSet>
{
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	public override PackableClassifierDataSet Convert(DataSet dataSet, ConversionSession session)
	{
		var packable = base.Convert(dataSet, session);
		packable.Tags = ConvertPlainTags(dataSet.TagsLibrary.Tags, session);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets, session);
		packable.Weights = ConvertPlainWeights(dataSet.WeightsLibrary.Weights, session);
		return packable;
	}

	private ImmutableArray<PackableClassifierAsset> ConvertAssets(IReadOnlyCollection<Asset> assets, ConversionSession session)
	{
		return assets.Cast<ClassifierAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableClassifierAsset ConvertAsset(ClassifierAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			session.TagsIds[asset.Tag]);
	}
}