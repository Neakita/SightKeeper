using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using Poser3DAsset = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DAsset;
using Poser3DItem = SightKeeper.Data.Binary.DataSets.Poser3D.Poser3DItem;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DAssetsConverter
{
	public Poser3DAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<Poser3DAsset> Convert(IEnumerable<Domain.Model.DataSets.Poser3D.Poser3DAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private Poser3DAsset Convert(Domain.Model.DataSets.Poser3D.Poser3DAsset asset, ConversionSession session)
	{
		return new Poser3DAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<Poser3DItem> Convert(IEnumerable<Domain.Model.DataSets.Poser3D.Poser3DItem> items, ConversionSession session)
	{
		return items.Select(item => Poser3DItem.Create(item, session)).ToImmutableArray();
	}

	private static ImmutableArray<Vector2<double>> Convert(IEnumerable<KeyPoint3D> keyPoints)
	{
		return keyPoints.Select(Convert).ToImmutableArray();
	}

	private static Vector2<double> Convert(KeyPoint3D keyPoint)
	{
		return keyPoint.Position;
	}
}