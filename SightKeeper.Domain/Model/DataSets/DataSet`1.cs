using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TAsset> : DataSet where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public sealed override ScreenshotsLibrary<TAsset> ScreenshotsLibrary { get; }
	public sealed override AssetsLibrary<TAsset> AssetsLibrary { get; }

	protected DataSet()
	{
		ScreenshotsLibrary = new ScreenshotsLibrary<TAsset>(this);
		AssetsLibrary = new AssetsLibrary<TAsset>(this);
	}
}