using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Classifier;

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
			TagIndex = _session.TagsIndexes[asset.Tag],
			Usage = asset.Usage,
			ScreenshotId = _screenshotsDataAccess.GetId(screenshot)
		};
	}
}