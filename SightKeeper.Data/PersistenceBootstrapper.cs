using MemoryPack;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

public static class PersistenceBootstrapper
{
	public static PersistenceServices Setup()
	{
		AppDataAccess appDataAccess = new();
		appDataAccess.Load();
		Lock editingLock = new();
		PeriodicDataSaver dataSaver = new()
		{
			DataSaver = appDataAccess
		};


		AppDataImageSetsRepository imageSetsRepository = new(editingLock, appDataAccess, dataSaver);
		AppDataDataSetsRepository dataSetsRepository = new(appDataAccess, dataSaver, editingLock);

		TrackingImageLookupper imageLookupper = new(imageSetsRepository, imageSetsRepository);

		PersistenceServices services = new()
		{
			WriteImageSetRepository = imageSetsRepository,
			ReadImageSetRepository = imageSetsRepository,
			ObservableImageSetRepository = imageSetsRepository,
			WriteDataSetRepository = dataSetsRepository,
			ReadDataSetRepository = dataSetsRepository,
			ObservableDataSetRepository = dataSetsRepository,
			ImageSetFactory = new StorableImageSetFactory(dataSaver, editingLock),
			ClassifierDataSetFactory = new StorableClassifierDataSetFactory(dataSaver, editingLock)
		};
		var imagesDataAccess = new CompressedFileSystemDataAccess();
		imagesDataAccess.DirectoryPath = Path.Combine(imagesDataAccess.DirectoryPath, "Images");
		MemoryPackFormatterProvider.Register(new ImageSetFormatter(new StorableImageSetWrapper(dataSaver, editingLock), new StorableImageWrapper(imagesDataAccess)));
		MemoryPackFormatterProvider.Register(new DataSetFormatter
		{
			ClassifierDataSetFormatter = new ClassifierDataSetFormatter
			{
				ImageLookupper = imageLookupper,
				SetWrapper = new ClassifierDataSetWrapper()
			}
		});

		return services;
	}
}