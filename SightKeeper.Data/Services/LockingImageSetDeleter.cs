using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class LockingImageSetDeleter : ImageSetDeleter
{
	public LockingImageSetDeleter(
		ReadRepository<DataSet> dataSetsRepository,
		WriteRepository<ImageSet> librariesRepository,
		[Tag(typeof(AppData))] Lock appDataLock) :
		base(dataSetsRepository, librariesRepository)
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