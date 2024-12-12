using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataDetectorAnnotator : DetectorAnnotator
{
	public AppDataDetectorAnnotator(AppDataAccess dataAccess, AppDataEditingLock editingLock)
	{
		_dataAccess = dataAccess;
		_lock = editingLock;
	}

	public override DetectorItem CreateItem(
		AssetsLibrary<DetectorAsset> assetsLibrary,
		Screenshot screenshot,
		Tag tag,
		Bounding bounding)
	{
		DetectorItem item;
		lock (_lock)
			item = base.CreateItem(assetsLibrary, screenshot, tag, bounding);
		_dataAccess.SetDataChanged();
		return item;
	}

	public override void SetBounding(DetectorItem item, Bounding bounding)
	{
		lock (_lock)
			base.SetBounding(item, bounding);
		_dataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _dataAccess;
	private readonly AppDataEditingLock _lock;
}