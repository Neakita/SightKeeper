using MemoryPack;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.Images;
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

		MemoryPackFormatterProvider.Register(new ImageSetFormatter(dataSaver, editingLock));
		MemoryPackFormatterProvider.Register(new DataSetFormatter(imageLookupper));

		return services;
	}
}