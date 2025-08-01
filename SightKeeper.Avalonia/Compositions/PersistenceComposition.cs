using System.IO;
using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Compositions;

public sealed class PersistenceComposition
{
	private void Setup() => DI.Setup(nameof(PersistenceComposition), CompositionKind.Internal)

		.Bind<WriteRepository<ImageSet>>()
		.Bind<ReadRepository<ImageSet>>()
		.Bind<ObservableRepository<ImageSet>>()
		.As(Lifetime.Singleton)
		.To<AppDataImageSetsRepository>()

		.Bind<WriteRepository<DataSet>>()
		.Bind<ReadRepository<DataSet>>()
		.Bind<ObservableRepository<DataSet>>()
		.As(Lifetime.Singleton)
		.To<AppDataDataSetsRepository>()

		.Bind<ImageWrapper>()
		.To<StorableImageWrapper>(_ =>
		{
			CompressedFileSystemDataAccess dataAccess = new()
			{
				DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images")
			};
			return new StorableImageWrapper(dataAccess);
		})

		.Bind<ImageSetWrapper>()
		.To<StorableImageSetWrapper>()

		.Bind<ImageSetFactory<ImageSet>>()
		.To<WrappedImageSetFactory>(context =>
		{
			context.Inject(out InMemoryImageSetFactory inMemoryImageSetFactory);
			context.Inject(out ImageSetWrapper wrapper);
			return new WrappedImageSetFactory(inMemoryImageSetFactory, wrapper);
		})

		.Bind<DataSetFactory<ClassifierDataSet>>()
		.Bind<InnerAwareDataSetFactory<StorableClassifierDataSet>>()
		.To<WrappingClassifierDataSetFactory>()
	
		.Bind<DataSetFactory<DetectorDataSet>>()
		.To<DataSetFactory<DetectorDataSet>>(_ => null!)
	
		.Bind<DataSetFactory<Poser2DDataSet>>()
		.To<DataSetFactory<Poser2DDataSet>>(_ => null!)
	
		.Bind<DataSetFactory<Poser3DDataSet>>()
		.To<DataSetFactory<Poser3DDataSet>>(_ => null!)

		.Bind<ChangeListener>()
		.As(Lifetime.Singleton)
		.To<PeriodicDataSaver>()

		.Bind<DataSaver>()
		.Bind<AppDataAccess>()
		.As(Lifetime.Singleton)
		.To<AppDataAccess>()

		.Bind<ImageSetFactory<InMemoryImageSet>>()
		.To<InMemoryImageSetFactory>()

		.Bind<ImageLookupperPopulator>()
		.Bind<ImageLookupper>()
		.As(Lifetime.Singleton)
		.To<PopulatableImageLookupper>()

		.Root<ImageSetFormatter>(nameof(ImageSetFormatter))
		.Root<DataSetFormatter>(nameof(DataSetFormatter))

		.Root<AppDataAccess>(nameof(AppDataAccess));
}