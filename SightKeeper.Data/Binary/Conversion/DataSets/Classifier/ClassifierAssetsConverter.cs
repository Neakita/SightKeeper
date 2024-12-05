using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Classifier;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_session = session;
	}

	public IEnumerable<PackableClassifierAsset> ConvertAssets(IEnumerable<KeyValuePair<Screenshot, ClassifierAsset>> assetsPairs)
	{
		return assetsPairs.Select(ConvertAsset);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ConversionSession _session;

	private PackableClassifierAsset ConvertAsset(KeyValuePair<Screenshot, ClassifierAsset> assetPair)
	{
		var (screenshot, asset) = assetPair;
		return new PackableClassifierAsset
		{
			TagId = _session.TagsIds[asset.Tag],
			Usage = asset.Usage,
			ScreenshotId = _screenshotsDataAccess.GetId(screenshot)
		};
	}
}