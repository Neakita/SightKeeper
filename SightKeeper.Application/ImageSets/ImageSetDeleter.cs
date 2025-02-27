using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public abstract class ImageSetDeleter
{
	public virtual bool CanDelete(ImageSet library)
	{
		var dataSets = _dataSetsDataAccess.Items;
		foreach (var dataSet in dataSets)
		foreach (var image in library.Images)
			if (dataSet.AssetsLibrary.Contains(image))
				return false;
		return true;
	}

	public virtual void Delete(ImageSet library)
	{
		Guard.IsTrue(CanDelete(library));
		_librariesDataAccess.Remove(library);
	}

	protected ImageSetDeleter(ReadDataAccess<DataSet> dataSetsDataAccess, WriteDataAccess<ImageSet> librariesDataAccess)
	{
		_dataSetsDataAccess = dataSetsDataAccess;
		_librariesDataAccess = librariesDataAccess;
	}

	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;
	private readonly WriteDataAccess<ImageSet> _librariesDataAccess;
}