using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal sealed class PoserAssetsConverter
{
	public PoserAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<SerializablePoserAsset> Convert(IEnumerable<PoserAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private SerializablePoserAsset Convert(PoserAsset asset, ConversionSession session)
	{
		return new SerializablePoserAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<SerializablePoserItem> Convert(IEnumerable<PoserItem> items, ConversionSession session)
	{
		return items.Select(item => new SerializablePoserItem(session.Tags[item.Tag], item.Bounding, Convert(item.KeyPoints))).ToImmutableArray();
	}

	private static ImmutableArray<Vector2<double>> Convert(IEnumerable<KeyPoint> keyPoints)
	{
		return keyPoints.Select(Convert).ToImmutableArray();
	}

	private static Vector2<double> Convert(KeyPoint keyPoint)
	{
		return keyPoint.Position;
	}
}