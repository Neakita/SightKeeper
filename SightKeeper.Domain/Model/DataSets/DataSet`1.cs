using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TAsset> : DataSet where TAsset : Asset
{
	public sealed override ScreenshotsLibrary<TAsset> ScreenshotsLibrary { get; }
	public abstract override AssetsLibrary<TAsset> AssetsLibrary { get; }

	protected DataSet()
	{
		ScreenshotsLibrary = new ScreenshotsLibrary<TAsset>(this);
	}
}