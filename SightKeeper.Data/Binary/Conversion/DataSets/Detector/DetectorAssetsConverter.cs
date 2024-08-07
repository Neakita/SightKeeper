using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using DetectorAsset = SightKeeper.Data.Binary.DataSets.Detector.DetectorAsset;
using DetectorItem = SightKeeper.Data.Binary.DataSets.Detector.DetectorItem;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Detector;

internal sealed class DetectorAssetsConverter
{
	public DetectorAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	internal ImmutableArray<DetectorAsset> Convert(IEnumerable<Domain.Model.DataSets.Detector.DetectorAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private DetectorAsset Convert(Domain.Model.DataSets.Detector.DetectorAsset asset, ConversionSession session)
	{
		return new DetectorAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<DetectorItem> Convert(IEnumerable<Domain.Model.DataSets.Detector.DetectorItem> items, ConversionSession session)
	{
		return items.Select(item => new DetectorItem(session.Tags[item.Tag], item.Bounding)).ToImmutableArray();
	}
}