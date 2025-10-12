using Autofac;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.DataSets.Poser;
using SightKeeper.Data.DataSets.Poser.Items.Decorators;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Data.DataSets.Poser.Tags;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets;
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
	public static void AddPersistence(this ContainerBuilder builder)
	{
		builder.AddRepositories();
		builder.AddWrappers();
		builder.AddFactories();
		builder.AddFormatters();
		builder.AddDeserializers();
		builder.AddPng();

		builder.RegisterType<AppDataAccess>()
			.SingleInstance()
			.AsSelf()
			.As<DataSaver>();

		builder.RegisterType<PopulatableImageLookupper>()
			.SingleInstance()
			.AsSelf()
			.As<ImageLookupperPopulator>()
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
	}

	private static void AddRepositories(this ContainerBuilder builder)
	{
		builder.AddImageSetsRepository();
		builder.AddDataSetsRepository();
	}

	private static void AddImageSetsRepository(this ContainerBuilder builder)
	{
		builder.RegisterType<AppDataImageSetsRepository>()
			.SingleInstance()
			.As<WriteRepository<ImageSet>>()
			.As<ReadRepository<ImageSet>>()
			.As<ObservableRepository<ImageSet>>();
	}

	private static void AddDataSetsRepository(this ContainerBuilder builder)
	{
		builder.RegisterType<AppDataDataSetsRepository>()
			.SingleInstance()
			.As<WriteRepository<DataSet<Tag, Asset>>>()
			.As<ReadRepository<DataSet<Tag, Asset>>>()
			.As<ObservableRepository<DataSet<Tag, Asset>>>();
	}

	private static void AddWrappers(this ContainerBuilder builder)
	{
		builder.RegisterType<StorableImageSetWrapper>()
			.As<ImageSetWrapper>();

		builder.RegisterType<KeyPointWrapper>();

		builder.AddDataSetWrapper<Tag, ClassifierAsset>(0);
		builder.AddDataSetWrapper<Tag, ItemsAsset<DetectorItem>>(1);
		builder.AddDataSetWrapper<PoserTag, ItemsAsset<PoserItem>>(2);
	}

	private static void AddFactories(this ContainerBuilder builder)
	{
		builder.RegisterType<InMemoryImageSetFactory>()
			.As<ImageSetFactory<ImageSet>>()
			.As<ImageSetFactory<InMemoryImageSet>>();

		builder.RegisterDecorator<WrappedImageSetFactory, ImageSetFactory<ImageSet>>();

		builder.RegisterType<WrappingClassifierDataSetFactory>()
			.As<DataSetFactory<Tag, ClassifierAsset>>();

		builder.RegisterType<WrappingDetectorDataSetFactory>()
			.As<DataSetFactory<Tag, ItemsAsset<DetectorItem>>>();

		builder.RegisterType<WrappingPoserDataSetFactory>()
			.As<DataSetFactory<PoserTag, ItemsAsset<PoserItem>>>();

		builder.RegisterType<InMemoryKeyPointFactory>()
			.As<KeyPointFactory>();
		
		builder.RegisterDecorator<WrappingKeyPointFactory, KeyPointFactory>();
	}

	private static void AddFormatters(this ContainerBuilder builder)
	{
		builder.RegisterType<TypeSwitchAssetsFormatter>()
			.As<AssetsFormatter<Asset>>();

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

		builder.RegisterType<ImageSetFormatter>();
		builder.RegisterType<DataSetFormatter>();
	}

	private static void AddDeserializers(this ContainerBuilder builder)
	{
		builder.RegisterType<MemoryPackDataSetDeserializer<Tag, ClassifierAsset>>()
			.As<MemoryPackDataSetDeserializer>();

		builder.RegisterType<MemoryPackDataSetDeserializer<Tag, ItemsAsset<DetectorItem>>>()
			.As<MemoryPackDataSetDeserializer>();

		builder.RegisterType<MemoryPackDataSetDeserializer<PoserTag, ItemsAsset<PoserItem>>>()
			.As<MemoryPackDataSetDeserializer>();
	}

	private static void AddPng(this ContainerBuilder builder)
	{
		builder.Register(_ =>
		{
			FileSystemDataAccess dataAccess = new()
			{
				DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images"),
				FileExtension = "png"
			};
			return new StorableImageWrapper(dataAccess);
		}).As<ImageWrapper>();

		builder.RegisterType<PngEncoder>()
			.As<IImageEncoder>();

		builder.RegisterGeneric(typeof(ImageSharpImageDataSaver<>))
			.As(typeof(ImageDataSaver<>));

		builder.RegisterGeneric(typeof(ImageSharpImageLoader<>))
			.As(typeof(ImageLoader<>));

		builder.RegisterGenericDecorator(typeof(ThreadedImageLoader<>), typeof(ImageLoader<>));
	}

	private static void AddDataSetWrapper<TTag, TAsset>(this ContainerBuilder builder, ushort unionTag)
		where TTag : Tag
	{
		builder.Register(context =>
		{
			var changeListener = context.Resolve<ChangeListener>();
			var editingLock = context.Resolve<Lock>();
			var tagsFormatter = context.Resolve<TagsFormatter<TTag>>();
			var assetsFormatter = context.Resolve<AssetsFormatter<TAsset>>(); 
			return new DataSetWrapper<TTag, TAsset>(
				changeListener,
				editingLock,
				unionTag,
				tagsFormatter,
				assetsFormatter);
		});
	}
}