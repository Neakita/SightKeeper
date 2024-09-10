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
	public ClassifierDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	public override PackableClassifierDataSet Convert(DataSet dataSet)
	{
		var packable = base.Convert(dataSet);
		packable.Tags = ConvertPlainTags(dataSet.TagsLibrary.Tags);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets);
		packable.Weights = ConvertPlainWeights(dataSet.WeightsLibrary.Weights);
		return packable;
	}

	private ImmutableArray<PackableClassifierAsset> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<ClassifierAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableClassifierAsset ConvertAsset(ClassifierAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			Session.TagsIds[asset.Tag]);
	}
}