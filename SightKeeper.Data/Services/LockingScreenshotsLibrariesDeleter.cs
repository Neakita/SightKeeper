using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class LockingScreenshotsLibrariesDeleter : ScreenshotsLibrariesDeleter
{
	public LockingScreenshotsLibrariesDeleter(
		ReadDataAccess<DataSet> dataSetsDataAccess,
		WriteDataAccess<ImageSet> librariesDataAccess,
		[Tag(typeof(AppData))] Lock appDataLock) :
		base(dataSetsDataAccess, librariesDataAccess)
	{
		_appDataLock = appDataLock;
	}

	public override bool CanDelete(ImageSet library)
	{
		lock (_appDataLock)
			return base.CanDelete(library);
	}

	public override void Delete(ImageSet library)
	{
		lock (_appDataLock)
			base.Delete(library);
	}

	private readonly Lock _appDataLock;
}