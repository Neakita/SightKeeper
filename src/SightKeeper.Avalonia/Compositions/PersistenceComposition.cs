using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.Images;
using DomainTag = SightKeeper.Domain.DataSets.Tags.Tag;

namespace SightKeeper.Avalonia.Compositions;

public sealed class PersistenceComposition
{
	private void Setup() => DI.Setup(nameof(PersistenceComposition), CompositionKind.Internal)

		.Bind<WriteRepository<ImageSet>>()
		.Bind<ReadRepository<ImageSet>>()
		.Bind<ObservableRepository<ImageSet>>()
		.As(Lifetime.Singleton)
		.To<AppDataImageSetsRepository>()

		.Bind<WriteRepository<DataSet<DomainTag, Asset>>>()
		.Bind<ReadRepository<DataSet<DomainTag, Asset>>>()
		.Bind<ObservableRepository<DataSet<DomainTag, Asset>>>()
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

		.Bind<DataSetFactory<DomainTag, ClassifierAsset>>()
		.To<WrappingClassifierDataSetFactory>()
	
		.Bind<DataSetFactory<DomainTag, ItemsAsset<DetectorItem>>>()
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
		
		.Bind<AssetsFormatter<Asset>>()
		.To<TypeSwitchAssetsFormatter>()
		
		.Bind<AssetsFormatter<ClassifierAsset>>()
		.To<ClassifierAssetsFormatter>()
		
		.Bind<AssetsFormatter<ItemsAsset<DetectorItem>>>()
		.To<ItemsAssetsFormatter<DetectorItem>>()
		
		.Bind<ItemsFormatter<DetectorItem>>()
		.To<DetectorItemsFormatter>()
		
		.Bind<MemoryPackDataSetDeserializer>(0)
		.To<MemoryPackDataSetDeserializer<DomainTag, ClassifierAsset>>()
		
		.Bind<MemoryPackDataSetDeserializer>(1)
		.To<MemoryPackDataSetDeserializer<DomainTag, ItemsAsset<DetectorItem>>>()
		
		/*.Bind<MemoryPackDataSetDeserializer>(2)
		.To<MemoryPackDataSetDeserializer<PoserTag, ItemsAsset<PoserItem>>>()*/
		
		.Bind<TagsFormatter<DomainTag>>()
		.To<PlainTagsFormatter>()
		
		.Bind<TagsFormatter<PoserTag>>()
		.To<PoserTagsFormatter>()
		
		/*.Bind().To<IReadOnlyList<MemoryPackDataSetDeserializer>>(context =>
		{
			context.Inject(out MemoryPackDataSetDeserializer<DomainTag, ClassifierAsset> classifierDeserializer);
			context.Inject(out MemoryPackDataSetDeserializer<DomainTag, ItemsAsset<DetectorItem>> detectorDeserializer);
			context.Inject(out MemoryPackDataSetDeserializer<PoserTag, ItemsAsset<PoserItem>> poserDeserializer);
			return
			[
				classifierDeserializer,
				detectorDeserializer,
				poserDeserializer
			];
		})*/

		.Root<ImageSetFormatter>(nameof(ImageSetFormatter))
		.Root<DataSetFormatter>(nameof(DataSetFormatter))

		.Root<AppDataAccess>(nameof(AppDataAccess));
}