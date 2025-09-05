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
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.Images;
using Tag = SightKeeper.Domain.DataSets.Tags.Tag;

namespace SightKeeper.Avalonia.Compositions;

public sealed class PersistenceComposition
{
	private void Setup() => DI.Setup(nameof(PersistenceComposition), CompositionKind.Internal)

		.Bind<WriteRepository<ImageSet>>()
		.Bind<ReadRepository<ImageSet>>()
		.Bind<ObservableRepository<ImageSet>>()
		.As(Lifetime.Singleton)
		.To<AppDataImageSetsRepository>()

		.Bind<WriteRepository<DataSet<Tag, Asset>>>()
		.Bind<ReadRepository<DataSet<Tag, Asset>>>()
		.Bind<ObservableRepository<DataSet<Tag, Asset>>>()
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

		.Bind<DataSetFactory<Tag, ClassifierAsset>>()
		.To<WrappingClassifierDataSetFactory>()
	
		.Bind<DataSetFactory<Tag, ItemsAsset<DetectorItem>>>()
		.To<WrappingDetectorDataSetFactory>()
	
		.Bind<DataSetFactory<PoserTag, ItemsAsset<PoserItem>>>()
		.To<DataSetFactory<PoserTag, ItemsAsset<PoserItem>>>(_ => null!)

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