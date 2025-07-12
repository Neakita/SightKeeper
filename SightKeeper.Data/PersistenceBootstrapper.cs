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

		var classifierSetWrapper = new ClassifierDataSetWrapper(dataSaver, editingLock);
        
        var imagesDataAccess = new CompressedFileSystemDataAccess();
        imagesDataAccess.DirectoryPath = Path.Combine(imagesDataAccess.DirectoryPath, "Images");
        var imageWrapper = new StorableImageWrapper(imagesDataAccess);
        var inMemoryImageSetFactory = new InMemoryImageSetFactory(imageWrapper);
        var imageSetWrapper = new StorableImageSetWrapper(dataSaver, editingLock);

		PersistenceServices services = new()
		{
			WriteImageSetRepository = imageSetsRepository,
			ReadImageSetRepository = imageSetsRepository,
			ObservableImageSetRepository = imageSetsRepository,
			WriteDataSetRepository = dataSetsRepository,
			ReadDataSetRepository = dataSetsRepository,
			ObservableDataSetRepository = dataSetsRepository,
			ImageSetFactory = new WrappedImageSetFactory(inMemoryImageSetFactory, imageSetWrapper),
			ClassifierDataSetFactory = new WrappingClassifierDataSetFactory(classifierSetWrapper)
		};

		MemoryPackFormatterProvider.Register(new ImageSetFormatter(imageSetWrapper, imageWrapper));
		MemoryPackFormatterProvider.Register(new DataSetFormatter
		{
			ClassifierDataSetFormatter = new ClassifierDataSetFormatter
			{
				ImageLookupper = imageLookupper,
				SetWrapper = classifierSetWrapper
			}
		});

		return services;
	}
}