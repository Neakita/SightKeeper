using Autofac;
using Serilog;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Decorators;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.DataSets.Poser;
using SightKeeper.Data.DataSets.Poser.Items.Decorators;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Data.DataSets.Poser.Tags;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace SightKeeper.Data;

public static class PersistenceExtensions
{
	public static void AddPersistence(this ContainerBuilder builder, PersistenceOptions options)
	{
		builder.AddRepositories();
		builder.AddWrappers(options);
		builder.AddFactories();
		builder.AddFormatters();
		builder.AddSerializers();
		builder.AddDeserializers();
		builder.AddPng();

		builder.RegisterType<MemoryPackDataSaver>()
			.As<DataSaver>();

		builder.RegisterType<TrackingImageLookupper>()
			.SingleInstance()
			.As<ImageLookupper>();

		builder.RegisterType<ZippedMemoryPackDataSetExporter>()
			.As<DataSetExporter<DataSet<Tag, Asset>>>();

		builder.RegisterType<ZippedMemoryPackDataSetImporter>()
			.As<DataSetImporter>();

		builder.RegisterType<Lock>()
			.SingleInstance();

		builder.RegisterType<PeriodicDataSaver>()
			.SingleInstance()
			.AsSelf()
			.As<ChangeListener>();

		builder.RegisterType<MemoryPackDataLoader>()
			.As<DataLoader>();

		builder.RegisterType<DecoratorTagIndexProvider>()
			.As<TagIndexProvider>();

		builder.RegisterType<DecoratorLifetimeScopeProvider>()
			.As<LifetimeScopeProvider>();
	}

	private static void AddRepositories(this ContainerBuilder builder)
	{
		builder.RegisterGeneric(typeof(InMemoryRepository<>))
			.SingleInstance()
			.As(typeof(Repository<>))
			.As(typeof(WriteRepository<>))
			.As(typeof(ReadRepository<>))
			.As(typeof(ObservableRepository<>))
			.As(typeof(ShortcutWriteRepository<>));
		builder.RegisterDecorator<EnsureCanDeleteImageSetRepository, Repository<ImageSet>>();
	}

	private static void AddWrappers(this ContainerBuilder builder, PersistenceOptions options)
	{
		builder.AddImageSetWrappers();

		builder.RegisterType<KeyPointWrapper>();

		builder.AddDataSetWrappers<Tag, ClassifierAsset>(0, 2, options.ClassifierDataSetScopeConfiguration, containerBuilder =>
		{
			containerBuilder.AddDataSetDecoratorWrapper<Tag, ClassifierAsset>(set =>
			{
				return new OverrideLibrariesDataSet<Tag, ClassifierAsset>(set)
				{
					AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
				};
			});
		});
		builder.AddDataSetWrappers<Tag, ItemsAsset<DetectorItem>>(1, 1, options.DetectorDataSetScopeConfiguration);
		builder.AddDataSetWrappers<PoserTag, ItemsAsset<PoserItem>>(2, 1, options.PoserDataSetScopeConfiguration);
	}

	private static void AddFactories(this ContainerBuilder builder)
	{
		builder.RegisterType<InMemoryImageSetFactory>()
			.As<Factory<ImageSet>>()
			.As<Factory<InMemoryImageSet>>();

		builder.RegisterDecorator<WrappedImageSetFactory, Factory<ImageSet>>();

		builder.RegisterType<WrappingClassifierDataSetFactory>()
			.As<Factory<DataSet<Tag, ClassifierAsset>>>();

		builder.RegisterType<WrappingDetectorDataSetFactory>()
			.As<Factory<DataSet<Tag, ItemsAsset<DetectorItem>>>>();

		builder.RegisterType<WrappingPoserDataSetFactory>()
			.As<Factory<DataSet<PoserTag, ItemsAsset<PoserItem>>>>();

		builder.RegisterType<InMemoryKeyPointFactory>()
			.As<KeyPointFactory>();
		
		builder.RegisterDecorator<WrappingKeyPointFactory, KeyPointFactory>();
	}

	private static void AddFormatters(this ContainerBuilder builder)
	{
		builder.RegisterType<ClassifierAssetsFormatter>()
			.As<AssetsFormatter<ClassifierAsset>>();

		builder.RegisterType<ItemsAssetsFormatter<DetectorItem>>()
			.As<AssetsFormatter<ItemsAsset<DetectorItem>>>();

		builder.RegisterType<ItemsAssetsFormatter<PoserItem>>()
			.As<AssetsFormatter<ItemsAsset<PoserItem>>>();

		builder.RegisterType<DetectorItemsFormatter>()
			.As<ItemsFormatter<DetectorItem>>();

		builder.RegisterType<PoserItemsFormatter>()
			.As<ItemsFormatter<PoserItem>>();

		builder.RegisterType<PlainTagsFormatter>()
			.As<TagsFormatter<Tag>>();

		builder.RegisterType<PoserTagsFormatter>()
			.As<TagsFormatter<PoserTag>>();

		builder.RegisterType<WeightsFormatter>();
	}

	private static void AddSerializers(this ContainerBuilder builder)
	{
		builder.RegisterType<ReadOnlyCollectionSerializer<ImageSet>>()
			.As<Serializer<IReadOnlyCollection<ImageSet>>>();

		builder.RegisterType<ImageSetSerializer>()
			.As<Serializer<ImageSet>>();

		builder.RegisterType<ReadOnlyCollectionSerializer<DataSet<Tag, Asset>>>()
			.As<Serializer<IReadOnlyCollection<DataSet<Tag, Asset>>>>();

		builder.RegisterType<DecoratorDataSetSerializer>()
			.As<Serializer<DataSet<Tag, Asset>>>();

		builder.RegisterType<ReadOnlyCollectionSerializer<ManagedImage>>()
			.As<Serializer<IReadOnlyCollection<ManagedImage>>>();

		builder.RegisterType<ImageSerializer>()
			.As<Serializer<ManagedImage>>();
	}

	private static void AddDeserializers(this ContainerBuilder builder)
	{
		builder.RegisterType<DataSetDeserializer<Tag, ClassifierAsset>>()
			.As<Deserializer<DataSet<Tag, Asset>>>();

		builder.RegisterType<DataSetDeserializer<Tag, ItemsAsset<DetectorItem>>>()
			.As<Deserializer<DataSet<Tag, Asset>>>();

		builder.RegisterType<DataSetDeserializer<PoserTag, ItemsAsset<PoserItem>>>()
			.As<Deserializer<DataSet<Tag, Asset>>>();

		builder.RegisterComposite<UnionTagSelectorDataSetDeserializer, Deserializer<DataSet<Tag, Asset>>>();

		builder.RegisterType<ReadOnlyCollectionDeserializer<ImageSet>>()
			.As<Deserializer<IEnumerable<ImageSet>>>();

		builder.RegisterType<ReadOnlyCollectionDeserializer<DataSet<Tag, Asset>>>()
			.As<Deserializer<IEnumerable<DataSet<Tag, Asset>>>>();

		builder.RegisterType<ImageSetDeserializer>()
			.As<Deserializer<ImageSet>>();

		builder.RegisterType<ReadOnlyCollectionDeserializer<ManagedImage>>()
			.As<Deserializer<IReadOnlyCollection<ManagedImage>>>();

		builder.RegisterType<ImageDeserializer>()
			.As<Deserializer<ManagedImage>>();
	}

	private static void AddPng(this ContainerBuilder builder)
	{
		builder.Register(_ =>
		{
			FileSystemDataAccess dataAccess = new(Log.ForContext<FileSystemDataAccess>())
			{
				DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images"),
				FileExtension = "png"
			};
			return new StorableImageWrapper(dataAccess);
		}).As<Wrapper<ManagedImage>>();

		builder.RegisterType<PngEncoder>()
			.As<IImageEncoder>();

		builder.RegisterGeneric(typeof(ImageSharpImageDataSaver<>))
			.As(typeof(ImageDataSaver<>));

		builder.RegisterGeneric(typeof(ImageSharpImageLoader<>))
			.As(typeof(ImageLoader<>));

		builder.RegisterGenericDecorator(typeof(ThreadedImageLoader<>), typeof(ImageLoader<>));
	}

	private static void AddImageSetWrappers(this ContainerBuilder builder)
	{
		// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
		// and then tracking decorator will send another notification.
		builder.AddImageSetDecoratorWrapper<TrackableImageSet>();

		// Locking of domain rules can be relatively computationally heavy,
		// for example when removing images range every image should be checked if it is used by some asset,
		// so locking appears only after domain rules validated.
		builder.AddImageSetDecoratorWrapper<LockingImageSet>();

		// Images data removing could be expansive (we can remove hundreds or thousands of images in one call, or do that often),
		// and there is no need in lock because lock should affect AppData only,
		// not the image files,
		// so it shouldn't be locked
		builder.AddImageSetDecoratorWrapper<ImagesDataRemovingImageSet>();

		builder.AddImageSetDecoratorWrapper<ObservableImagesImageSet>();

		// We shouldn't dispose images if domain rule is violated,
		// so this should be behind domain rules
		builder.AddImageSetDecoratorWrapper<DisposingImageSet>();
		
		// will be needed when repository tries to remove the set
		builder.AddImageSetDecoratorWrapper<DeletableDataImageSet>();

		// If domain rule is violated and throws an exception,
		// it should fail as fast as possible and have smaller stack strace
		builder.AddImageSetDecoratorWrapper<DomainImageSet>();

		// INPC interface should be exposed to consumer,
		// so he can type test and cast it,
		// so it should be the outermost layer
		builder.AddImageSetDecoratorWrapper<NotifyingImageSet>();

		builder.RegisterComposite<CompositeWrapper<ImageSet>, Wrapper<ImageSet>>();
	}

	private static void AddImageSetDecoratorWrapper<TDecorator>(this ContainerBuilder builder)
		where TDecorator : ImageSet
	{
		builder.RegisterType<TDecorator>();
		builder.RegisterType<FuncWrapper<TDecorator, ImageSet>>()
			.As<Wrapper<ImageSet>>();
	}

	private static void AddDataSetWrappers<TTag, TAsset>(
		this ContainerBuilder builder,
		ushort unionTag,
		byte minimumTagsCount,
		Action<ContainerBuilder>? setScopeConfiguration,
		Action<ContainerBuilder>? additional = null)
		where TTag : Tag
	{
		// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
		// and then tracking decorator will send another notification.
		builder.AddDataSetDecoratorWrapper<TrackableDataSet<TTag, TAsset>, TTag, TAsset>();

		// Locking of domain rules can be relatively computationally heavy,
		// for example when removing images range every image should be checked if it is used by some asset,
		// so locking appears only after domain rules validated.
		builder.AddDataSetDecoratorWrapper<LockingDataSet<TTag, TAsset>, TTag, TAsset>();

		// Weights data removing could be expansive (we can remove large weights files),
		// and there is no need in lock because lock should affect AppData only,
		// not the weights files,
		// so it shouldn't be locked
		builder.AddDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				WeightsLibrary = new DataRemovingWeightsLibrary(set.WeightsLibrary)
			};
		});

		builder.AddDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				TagsLibrary = new ObservableTagsLibrary<TTag>(set.TagsLibrary),
				AssetsLibrary = new ObservableAssetsLibrary<TAsset>(set.AssetsLibrary),
				WeightsLibrary = new ObservableWeightsLibrary(set.WeightsLibrary)
			};
		});
		
		builder.AddDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				TagsLibrary = new IndexedTagTrackingTagsLibrary<TTag>(set.TagsLibrary)
			};
		});

		builder.Register(context =>
		{
			var tagsFormatter = context.Resolve<TagsFormatter<TTag>>();
			var assetsFormatter = context.Resolve<AssetsFormatter<TAsset>>();
			var weightsFormatter = context.Resolve<WeightsFormatter>();
			return new FuncWrapper<DataSet<TTag, TAsset>, DataSet<TTag, TAsset>>(set => 
			{
				return new SerializableDataSet<TTag, TAsset>(set, unionTag, tagsFormatter, assetsFormatter, weightsFormatter);
			});
		}).As<Wrapper<DataSet<TTag, TAsset>>>();

		builder.Register(context =>
		{
			var contextScope = context.Resolve<ILifetimeScope>();
			return new FuncWrapper<DataSet<TTag, TAsset>, DataSet<TTag, TAsset>>(set =>
			{
				return new DataSetLifetimeScopeProviderDecorator<TTag, TAsset>(set, contextScope, setScopeConfiguration);
			});
		}).As<Wrapper<DataSet<TTag, TAsset>>>();

		additional?.Invoke(builder);

		// If domain rule is violated and throws an exception,
		// it should fail as fast as possible and have smaller stack strace
		builder.AddDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new DomainDataSet<TTag, TAsset>(set, minimumTagsCount);
		});

		// INPC interface should be exposed to consumer,
		// so he can type test and cast it,
		// so it should be the outermost layer
		builder.AddDataSetDecoratorWrapper<NotifyingDataSet<TTag, TAsset>, TTag, TAsset>();

		builder.RegisterComposite<CompositeWrapper<DataSet<TTag, TAsset>>, Wrapper<DataSet<TTag, TAsset>>>();
	}

	private static void AddDataSetDecoratorWrapper<TDecorator, TTag, TAsset>(this ContainerBuilder builder)
		where TDecorator : DataSet<TTag, TAsset>
	{
		builder.RegisterType<TDecorator>();
		builder.RegisterType<FuncWrapper<TDecorator, DataSet<TTag, TAsset>>>()
			.As<Wrapper<DataSet<TTag, TAsset>>>();
	}

	private static void AddDataSetDecoratorWrapper<TTag, TAsset>(this ContainerBuilder builder, Func<DataSet<TTag, TAsset>, DataSet<TTag,TAsset>> func)
	{
		var wrapper = new FuncWrapper<DataSet<TTag, TAsset>, DataSet<TTag, TAsset>>(func);
		builder.RegisterInstance(wrapper)
			.As<Wrapper<DataSet<TTag, TAsset>>>();
	}
}