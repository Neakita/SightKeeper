using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public abstract class ScreenshotsLibrariesDeleter
{
	public virtual bool CanDelete(ImageSet library)
	{
		var dataSets = _dataSetsDataAccess.Items;
		foreach (var dataSet in dataSets)
		foreach (var screenshot in library.Images)
			if (dataSet.AssetsLibrary.Contains(screenshot))
				return false;
		return true;
	}

	public virtual void Delete(ImageSet library)
	{
		Guard.IsTrue(CanDelete(library));
		_librariesDataAccess.Remove(library);
	}

	protected ScreenshotsLibrariesDeleter(ReadDataAccess<DataSet> dataSetsDataAccess, WriteDataAccess<ImageSet> librariesDataAccess)
	{
		_dataSetsDataAccess = dataSetsDataAccess;
		_librariesDataAccess = librariesDataAccess;
	}

	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;
	private readonly WriteDataAccess<ImageSet> _librariesDataAccess;
}