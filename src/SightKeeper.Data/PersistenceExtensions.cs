using Autofac;
using Autofac.Builder;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.Misc;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Artifacts;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Decorators;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Poser.Items;
using SightKeeper.Data.DataSets.Poser.Items.Decorators;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Data.DataSets.Poser.Tags;
using SightKeeper.Data.DataSets.Tags;
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
	public static void RegisterPersistence(this ContainerBuilder builder, PersistenceOptions options)
	{
		builder.RegisterRepositories();
		builder.RegisterWrappers(options);
		builder.RegisterFactories();
		builder.RegisterFormatters();
		builder.RegisterSerializers();
		builder.RegisterDeserializers();
		builder.RegisterPng();

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

	private static void RegisterRepositories(this ContainerBuilder builder)
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

	private static void RegisterWrappers(this ContainerBuilder builder, PersistenceOptions options)
	{
		builder.RegisterImageSetWrappers();
		
		builder.RegisterType<StorableImageWrapper>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.ResolveNamed<FileSystemDataAccess>("images"))
			.As<Wrapper<ManagedImage>>();

		builder.RegisterType<KeyPointWrapper>();

		builder.RegisterDataSetWrappers<Tag, ClassifierAsset>(0, options.ClassifierDataSetScopeConfiguration, containerBuilder =>
		{
			containerBuilder.RegisterDataSetDecoratorWrapper<Tag, ClassifierAsset>(set =>
			{
				return new OverrideLibrariesDataSet<Tag, ClassifierAsset>(set)
				{
					AssetsLibrary = new TagUsersTrackingClassifierAssetsLibrary(set.AssetsLibrary)
				};
			});
		});
		builder.RegisterDataSetWrappers<Tag, ItemsAsset<DetectorItem>>(1, options.DetectorDataSetScopeConfiguration);
		builder.RegisterDataSetWrappers<PoserTag, ItemsAsset<PoserItem>>(2, options.PoserDataSetScopeConfiguration);
	}

	private static void RegisterFactories(this ContainerBuilder builder)
	{
		builder.RegisterType<InMemoryImageSetFactory>()
			.As<Factory<ImageSet>>()
			.As<Factory<InMemoryImageSet>>();

		builder.RegisterDecorator<WrappedImageSetFactory, Factory<ImageSet>>();

		builder.RegisterType<InMemoryDataSetFactory<Tag, ClassifierAsset>>()
			.As<Factory<InMemoryDataSet<Tag, ClassifierAsset>>>();

		builder.RegisterType<WrappingFactory<DataSet<Tag, ClassifierAsset>>>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.Resolve<Factory<InMemoryDataSet<Tag, ClassifierAsset>>>())
			.As<Factory<DataSet<Tag, ClassifierAsset>>>();

		builder.RegisterType<InMemoryDataSetFactory<Tag, ItemsAsset<DetectorItem>>>()
			.As<Factory<InMemoryDataSet<Tag, ItemsAsset<DetectorItem>>>>();

		builder.RegisterType<WrappingFactory<DataSet<Tag, ItemsAsset<DetectorItem>>>>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.Resolve<Factory<InMemoryDataSet<Tag, ItemsAsset<DetectorItem>>>>())
			.As<Factory<DataSet<Tag, ItemsAsset<DetectorItem>>>>();

		builder.RegisterType<InMemoryDataSetFactory<PoserTag, ItemsAsset<PoserItem>>>()
			.As<Factory<InMemoryDataSet<PoserTag, ItemsAsset<PoserItem>>>>();

		builder.RegisterType<WrappingFactory<DataSet<PoserTag, ItemsAsset<PoserItem>>>>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.Resolve<Factory<InMemoryDataSet<PoserTag, ItemsAsset<PoserItem>>>>())
			.As<Factory<DataSet<PoserTag, ItemsAsset<PoserItem>>>>();

		builder.RegisterType<InMemoryKeyPointFactory>()
			.As<KeyPointFactory>();

		builder.RegisterDecorator<WrappingKeyPointFactory, KeyPointFactory>();

		builder.RegisterType<StorableTagFactory>()
			.As<TagFactory<Tag>>();

		builder.RegisterType<StorableClassifierAssetFactory>()
			.As<AssetFactory<ClassifierAsset>>();

		builder.RegisterType<StorableItemsAssetFactory<DetectorItem>>()
			.As<AssetFactory<ItemsAsset<DetectorItem>>>();

		builder.RegisterType<StorableDetectorItemFactory>()
			.As<Factory<DetectorItem>>();

		builder.RegisterType<StorableItemsAssetFactory<PoserItem>>()
			.As<AssetFactory<ItemsAsset<PoserItem>>>();

		builder.RegisterType<StorablePoserItemFactory>()
			.As<Factory<PoserItem>>();

		builder.RegisterType<StorablePoserTagFactory>()
			.As<TagFactory<PoserTag>>();

		builder.RegisterType<FileSystemDataAccess>()
			.WithParameter(new PositionalParameter(0, string.Empty))
			.OnActivating(args => args.Instance.DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Artifacts"))
			.Named<FileSystemDataAccess>("artifacts");
	}

	private static void RegisterFormatters(this ContainerBuilder builder)
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
	}

	private static void RegisterSerializers(this ContainerBuilder builder)
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

	private static void RegisterDeserializers(this ContainerBuilder builder)
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

	private static void RegisterPng(this ContainerBuilder builder)
	{
		builder.RegisterType<FileSystemDataAccess>()
			.WithParameter(new PositionalParameter(0, "png"))
			.OnActivating(args => args.Instance.DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images"))
			.Named<FileSystemDataAccess>("images");

		builder.RegisterType<PngEncoder>()
			.As<IImageEncoder>();

		builder.RegisterGeneric(typeof(ImageSharpImageDataSaver<>))
			.As(typeof(ImageDataSaver<>));

		builder.RegisterGeneric(typeof(ImageSharpImageLoader<>))
			.As(typeof(ImageLoader<>));

		builder.RegisterGenericDecorator(typeof(ThreadedImageLoader<>), typeof(ImageLoader<>));
	}

	private static void RegisterImageSetWrappers(this ContainerBuilder builder)
	{
		// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
		// and then tracking decorator will send another notification.
		builder.RegisterImageSetDecoratorWrapper<TrackableImageSet>();

		// Locking of domain rules can be relatively computationally heavy,
		// for example when removing images range every image should be checked if it is used by some asset,
		// so locking appears only after domain rules validated.
		builder.RegisterImageSetDecoratorWrapper<LockingImageSet>();

		// Images data removing could be expansive (we can remove hundreds or thousands of images in one call, or do that often),
		// and there is no need in lock because lock should affect AppData only,
		// not the image files,
		// so it shouldn't be locked
		builder.RegisterImageSetDecoratorWrapper<ImagesDataRemovingImageSet>();

		builder.RegisterImageSetDecoratorWrapper<ObservableImagesImageSet>();

		// We shouldn't dispose images if domain rule is violated,
		// so this should be behind domain rules
		builder.RegisterImageSetDecoratorWrapper<DisposingImageSet>();
		
		// will be needed when repository tries to remove the set
		builder.RegisterImageSetDecoratorWrapper<DeletableDataImageSet>();

		// If domain rule is violated and throws an exception,
		// it should fail as fast as possible and have smaller stack strace
		builder.RegisterImageSetDecoratorWrapper<DomainImageSet>();

		// INPC interface should be exposed to consumer,
		// so he can type test and cast it,
		// so it should be the outermost layer
		builder.RegisterImageSetDecoratorWrapper<NotifyingImageSet>();

		builder.RegisterComposite<CompositeWrapper<ImageSet>, Wrapper<ImageSet>>();
	}

	private static void RegisterImageSetDecoratorWrapper<TDecorator>(this ContainerBuilder builder)
		where TDecorator : ImageSet
	{
		builder.RegisterType<TDecorator>().ExternallyOwned();
		builder.RegisterType<FuncWrapper<TDecorator, ImageSet>>()
			.As<Wrapper<ImageSet>>();
	}

	private static void RegisterDataSetWrappers<TTag, TAsset>(
		this ContainerBuilder builder,
		ushort unionTag,
		Action<ContainerBuilder>? setScopeConfiguration,
		Action<ContainerBuilder>? additional = null)
		where TTag : Tag
	{
		// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
		// and then tracking decorator will send another notification.
		builder.RegisterDataSetDecoratorWrapper<TrackableDataSet<TTag, TAsset>, TTag, TAsset>();

		// Locking of domain rules can be relatively computationally heavy,
		// for example when removing images range every image should be checked if it is used by some asset,
		// so locking appears only after domain rules validated.
		builder.RegisterDataSetDecoratorWrapper<LockingDataSet<TTag, TAsset>, TTag, TAsset>();

		// Artifacts data removing could be expansive (we can remove large artifacts files),
		// and there is no need in lock because lock should affect AppData only,
		// not the artifacts files,
		// so it shouldn't be locked
		builder.RegisterDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				ArtifactsLibrary = new DataRemovingArtifactsLibrary(set.ArtifactsLibrary)
			};
		});

		builder.RegisterDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				TagsLibrary = new ObservableTagsLibrary<TTag>(set.TagsLibrary),
				AssetsLibrary = new ObservableAssetsLibrary<TAsset>(set.AssetsLibrary),
				ArtifactsLibrary = new ObservableArtifactsLibrary(set.ArtifactsLibrary)
			};
		});
		
		builder.RegisterDataSetDecoratorWrapper<TTag, TAsset>(set =>
		{
			return new OverrideLibrariesDataSet<TTag, TAsset>(set)
			{
				TagsLibrary = new IndexedTagTrackingTagsLibrary<TTag>(set.TagsLibrary)
			};
		});

		builder.RegisterDataSetDecoratorWrapper<SerializableDataSet<TTag, TAsset>, TTag, TAsset>()
			.WithParameter(new PositionalParameter(1, unionTag));

		builder.RegisterDataSetDecoratorWrapper<DataSetLifetimeScopeProviderDecorator<TTag, TAsset>, TTag, TAsset>()
			.WithParameter(new PositionalParameter(1, setScopeConfiguration));

		additional?.Invoke(builder);

		// If domain rule is violated and throws an exception,
		// it should fail as fast as possible and have smaller stack strace
		builder.RegisterDataSetDecoratorWrapper<DomainDataSet<TTag, TAsset>, TTag, TAsset>();

		// INPC interface should be exposed to consumer,
		// so he can type test and cast it,
		// so it should be the outermost layer
		builder.RegisterDataSetDecoratorWrapper<NotifyingDataSet<TTag, TAsset>, TTag, TAsset>();

		builder.RegisterComposite<CompositeWrapper<DataSet<TTag, TAsset>>, Wrapper<DataSet<TTag, TAsset>>>();
	}

	private static IRegistrationBuilder<TDecorator, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterDataSetDecoratorWrapper<TDecorator, TTag, TAsset>(this ContainerBuilder builder)
		where TDecorator : DataSet<TTag, TAsset>
	{
		builder.RegisterType<FuncWrapper<TDecorator, DataSet<TTag, TAsset>>>()
			.As<Wrapper<DataSet<TTag, TAsset>>>();
		return builder.RegisterType<TDecorator>();
	}

	private static void RegisterDataSetDecoratorWrapper<TTag, TAsset>(this ContainerBuilder builder, Func<DataSet<TTag, TAsset>, DataSet<TTag,TAsset>> func)
	{
		var wrapper = new FuncWrapper<DataSet<TTag, TAsset>, DataSet<TTag, TAsset>>(func);
		builder.RegisterInstance(wrapper)
			.As<Wrapper<DataSet<TTag, TAsset>>>();
	}
}