using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public abstract class ImageSetDeleter
{
	public virtual bool CanDelete(ImageSet library)
	{
		var dataSets = _dataSetsRepository.Items;
		foreach (var dataSet in dataSets)
		foreach (var image in library.Images)
			if (dataSet.AssetsLibrary.Contains(image))
				return false;
		return true;
	}

	public virtual void Delete(ImageSet library)
	{
		Guard.IsTrue(CanDelete(library));
		_librariesRepository.Remove(library);
	}

	protected ImageSetDeleter(ReadRepository<DataSet> dataSetsRepository, WriteRepository<ImageSet> librariesRepository)
	{
		_dataSetsRepository = dataSetsRepository;
		_librariesRepository = librariesRepository;
	}

	private readonly ReadRepository<DataSet> _dataSetsRepository;
	private readonly WriteRepository<ImageSet> _librariesRepository;
}