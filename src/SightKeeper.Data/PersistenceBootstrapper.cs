namespace SightKeeper.Data;

public static class PersistenceBootstrapper
{
	/*public static PersistenceServices Setup()
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

		PopulatableImageLookupper imageLookupper = new();

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

		MemoryPackFormatterProvider.Register(new ImageSetFormatter(imageSetWrapper, inMemoryImageSetFactory, imageLookupper));
		MemoryPackFormatterProvider.Register(new DataSetFormatter
		{
			ClassifierDataSetFormatter = new ClassifierDataSetFormatter
			{
				ImageLookupper = imageLookupper,
				SetWrapper = classifierSetWrapper
			}
		});

		return services;
	}*/
}