using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Services;

public sealed class AppDataPoser3DAnnotator : Poser3DAnnotator
{
	public AppDataPoser3DAnnotator(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public override Poser3DItem CreateItem(AssetsLibrary<Poser3DAsset> assetsLibrary, Screenshot screenshot,
		PoserTag tag, Bounding bounding)
	{
		Poser3DItem item;
		lock (_appDataLock)
			item = base.CreateItem(assetsLibrary, screenshot, tag, bounding);
		_appDataAccess.SetDataChanged();
		return item;
	}

	public override void SetBounding(DetectorItem item, Bounding bounding)
	{
		lock (_appDataLock)
			base.SetBounding(item, bounding);
		_appDataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
}