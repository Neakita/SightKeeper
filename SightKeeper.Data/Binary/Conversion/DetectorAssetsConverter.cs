using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Conversion;

public sealed class DetectorAssetsConverter
{
	public DetectorAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	internal ImmutableArray<SerializableDetectorAsset> Convert(IEnumerable<DetectorAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private SerializableDetectorAsset Convert(DetectorAsset asset, ConversionSession session)
	{
		return new SerializableDetectorAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<SerializableDetectorItem> Convert(IEnumerable<DetectorItem> items, ConversionSession session)
	{
		return items.Select(item => new SerializableDetectorItem(session.Tags[item.Tag], item.Bounding)).ToImmutableArray();
	}
}