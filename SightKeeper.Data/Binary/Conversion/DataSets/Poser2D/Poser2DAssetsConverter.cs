using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using Poser2DAsset = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DAsset;
using Poser2DItem = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DItem;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DAssetsConverter
{
	public Poser2DAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<Poser2DAsset> Convert(IEnumerable<Domain.Model.DataSets.Poser2D.Poser2DAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private Poser2DAsset Convert(Domain.Model.DataSets.Poser2D.Poser2DAsset asset, ConversionSession session)
	{
		return new Poser2DAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<Poser2DItem> Convert(IEnumerable<Domain.Model.DataSets.Poser2D.Poser2DItem> items, ConversionSession session)
	{
		return items.Select(item => new Poser2DItem(session.Tags[item.Tag], item.Bounding, item.KeyPoints, item.Properties)).ToImmutableArray();
	}
}