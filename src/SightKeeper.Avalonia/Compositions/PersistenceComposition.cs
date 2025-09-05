using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
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

		.Bind<WriteRepository<DataSet<Asset>>>()
		.Bind<ReadRepository<DataSet<Asset>>>()
		.Bind<ObservableRepository<DataSet<Asset>>>()
		.As(Lifetime.Singleton)
		.To<AppDataDataSetsRepository>()

		.Bind<ImageSetWrapper>()
		.To<StorableImageSetWrapper>()

		.Bind<ImageSetFactory<ImageSet>>()
		.To<WrappedImageSetFactory>(context =>
		{
			context.Inject(out InMemoryImageSetFactory inMemoryImageSetFactory);
			context.Inject(out ImageSetWrapper wrapper);
			return new WrappedImageSetFactory(inMemoryImageSetFactory, wrapper);
		})

		.Bind<DataSetFactory<ClassifierAsset>>()
		.To<WrappingClassifierDataSetFactory>()
	
		.Bind<DataSetFactory<ItemsAsset<DetectorItem>>>()
		.To<WrappingDetectorDataSetFactory>()
	
		.Bind<DataSetFactory<PoserDataSet>>()
		.To<DataSetFactory<PoserDataSet>>(_ => null!)

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