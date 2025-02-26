using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public abstract class ScreenshotsLibrariesDeleter
{
	public virtual bool CanDelete(ScreenshotsLibrary library)
	{
		var dataSets = _dataSetsDataAccess.Items;
		foreach (var dataSet in dataSets)
		foreach (var screenshot in library.Screenshots)
			if (dataSet.AssetsLibrary.Contains(screenshot))
				return false;
		return true;
	}

	public virtual void Delete(ScreenshotsLibrary library)
	{
		Guard.IsTrue(CanDelete(library));
		_librariesDataAccess.Remove(library);
	}

	protected ScreenshotsLibrariesDeleter(ReadDataAccess<DataSet> dataSetsDataAccess, WriteDataAccess<ScreenshotsLibrary> librariesDataAccess)
	{
		_dataSetsDataAccess = dataSetsDataAccess;
		_librariesDataAccess = librariesDataAccess;
	}

	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;
	private readonly WriteDataAccess<ScreenshotsLibrary> _librariesDataAccess;
}