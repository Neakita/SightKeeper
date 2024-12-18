using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Services;

public sealed class AppDataDetectorAnnotator : DetectorAnnotator
{
	public AppDataDetectorAnnotator(AppDataAccess dataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_dataAccess = dataAccess;
		_appDataLock = appDataLock;
	}

	public override DetectorItem CreateItem(
		AssetsLibrary<DetectorAsset> assetsLibrary,
		Screenshot screenshot,
		Tag tag,
		Bounding bounding)
	{
		DetectorItem item;
		lock (_appDataLock)
			item = base.CreateItem(assetsLibrary, screenshot, tag, bounding);
		_dataAccess.SetDataChanged();
		return item;
	}

	public override void SetBounding(DetectorItem item, Bounding bounding)
	{
		lock (_appDataLock)
			base.SetBounding(item, bounding);
		_dataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _dataAccess;
	private readonly Lock _appDataLock;
}