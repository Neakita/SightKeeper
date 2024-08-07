using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DAssetsConverter
{
	public Poser2DAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<SerializablePoser2DAsset> Convert(IEnumerable<Poser2DAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private SerializablePoser2DAsset Convert(Poser2DAsset asset, ConversionSession session)
	{
		return new SerializablePoser2DAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<SerializablePoser2DItem> Convert(IEnumerable<Poser2DItem> items, ConversionSession session)
	{
		return items.Select(item => new SerializablePoser2DItem(session.Tags[item.Tag], item.Bounding, item.KeyPoints, item.Properties)).ToImmutableArray();
	}
}