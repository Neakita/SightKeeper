using System;
using Autofac;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Classifier;
using SightKeeper.Avalonia.Annotation.Tooling.Detector;
using SightKeeper.Avalonia.Annotation.Tooling.Poser;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Avalonia.Training;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Infrastructure;

internal static class ViewModelsExtensions
{
	public static void RegisterViewModels(this ContainerBuilder builder)
	{
		builder.RegisterGeneral();
		builder.RegisterTabs();
		builder.RegisterImageSetsTabDependencies();
		builder.RegisterDataSetsTabDependencies();
		builder.RegisterAnnotationTabDependencies();
		builder.RegisterTrainingTabDependencies();
	}

	private static void RegisterGeneral(this ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
	}

	private static void RegisterImageSetsTabDependencies(this ContainerBuilder builder)
	{
		builder.Register(context =>
		{
			var createImageSetCommand = context.Resolve<CreateImageSetCommand>();
			var imageSetRepository = context.Resolve<ObservableListRepository<ImageSet>>();
			var imageSetCardDataContextFactory = context.Resolve<ImageSetCardDataContextFactory>();
			var capturingSettingsDataContext = context.Resolve<CapturingSettingsDataContext>();
			return new ImageSetsViewModel(
				createImageSetCommand,
				imageSetRepository,
				imageSetCardDataContextFactory,
				capturingSettingsDataContext);
		});

		builder.RegisterType<ImageSetCardViewModelFactory>()
			.As<ImageSetCardDataContextFactory>();

		builder.RegisterType<CapturingSettingsViewModel>()
			.As<CapturingSettingsDataContext>();

		builder.Register(context =>
		{
			var createImageSetCommand = context.Resolve<CreateImageSetCommand>();
			var imageSetsListRepository = context.Resolve<ObservableListRepository<ImageSet>>();
			var imageSetCardDataContextFactory = context.Resolve<ImageSetCardDataContextFactory>();
			var capturingSettings = context.Resolve<CapturingSettingsDataContext>();
			return new ImageSetsViewModel(
				createImageSetCommand,
				imageSetsListRepository,
				imageSetCardDataContextFactory,
				capturingSettings);
		});
	}

	private static void RegisterDataSetsTabDependencies(this ContainerBuilder builder)
	{
		builder.RegisterType<CreateDataSetViewModel>();
		builder.RegisterType<DataSetTypePickerViewModel>();

		builder.Register(context =>
		{
			var editDataSetCommand = context.Resolve<EditDataSetCommand>();
			var exportDataSetCommand = context.Resolve<ExportDataSetCommand>();
			var deleteDataSetCommand = context.Resolve<DeleteDataSetCommand>();
			var imageLoader = context.Resolve<WriteableBitmapImageLoader>();
			return new Func<DataSet<Tag, Asset>, DataSetCardViewModel>(dataSet =>
			{
				return new DataSetCardViewModel(
					dataSet,
					editDataSetCommand.WithParameter(dataSet),
					exportDataSetCommand.WithParameter(dataSet),
					deleteDataSetCommand.WithParameter(dataSet),
					imageLoader);
			});
		});

		builder.Register(context =>
		{
			var dataSetsObservableRepository = context.Resolve<ObservableListRepository<DataSet<Tag, Asset>>>();
			var createDataSetCommand = context.Resolve<CreateDataSetCommand>();
			var importDataSetCommand = context.Resolve<ImportDataSetCommand>();
			var dataSetCardViewModelFactory = context.Resolve<Func<DataSet<Tag, Asset>, DataSetCardViewModel>>();
			return new DataSetsViewModel(
				dataSetsObservableRepository,
				createDataSetCommand,
				importDataSetCommand,
				dataSetCardViewModelFactory);
		});

		builder.AddDataSetType<Tag, ClassifierAsset, PlainTagsEditorViewModel>("Classifier");
		builder.AddDataSetType<Tag, ItemsAsset<DetectorItem>, PlainTagsEditorViewModel>("Detector");
		builder.AddDataSetType<PoserTag, ItemsAsset<PoserItem>, PoserTagsEditorViewModel>("Poser");

		builder.RegisterType<PlainTagsEditorViewModel>();
		builder.RegisterType<PoserTagsEditorViewModel>();
	}

	private static void RegisterAnnotationTabDependencies(this ContainerBuilder builder)
	{
		builder.RegisterType<SideBarViewModel>()
			.SingleInstance()
			.As<AdditionalToolingSelection>()
			.As<SideBarDataContext>();

		builder.RegisterType<ImagesViewModel>()
			.SingleInstance()
			.As<ImagesDataContext>()
			.As<ImageSelection>();

		builder.RegisterType<ImageSetSelectionViewModel>()
			.SingleInstance()
			.As<ImageSetSelectionDataContext>()
			.As<ImageSetSelection>();

		builder.RegisterType<DataSetSelectionViewModel>()
			.SingleInstance()
			.As<DataSetSelection>()
			.As<DataSetSelectionDataContext>();

		builder.RegisterType<DrawerViewModel>()
			.SingleInstance()
			.As<DrawerDataContext>()
			.As<SelectedItemProvider>();

		builder.RegisterType<ActionsViewModel>()
			.As<ActionsDataContext>();

		builder.RegisterType<BoundingDrawerViewModel>();
		builder.RegisterType<AssetItemsViewModel>();
		builder.RegisterType<KeyPointDrawerViewModel>();
		builder.RegisterType<ClassifierToolingViewModel>();
		builder.RegisterType<DetectorToolingViewModel>();
		builder.RegisterType<PoserToolingViewModel>();
		builder.RegisterType<AnnotationTabViewModel>();
		builder.RegisterType<AnnotationImageViewModel>();
	}

	private static void RegisterTrainingTabDependencies(this ContainerBuilder builder)
	{
		builder.RegisterType<TrainingViewModel>();
	}

	private static void RegisterTabs(this ContainerBuilder builder)
	{
		builder.AddTabItemViewModel<ImageSetsViewModel>(MaterialIconKind.FolderMultipleImage, "Images");
		builder.AddTabItemViewModel<DataSetsViewModel>(MaterialIconKind.ImageAlbum, "Datasets");
		builder.AddTabItemViewModel<AnnotationTabViewModel>(MaterialIconKind.ImageEdit, "Annotation");
		builder.AddTabItemViewModel<TrainingViewModel>(MaterialIconKind.School, "Training");
	}

	private static void AddTabItemViewModel<TContent>(
		this ContainerBuilder builder,
		MaterialIconKind iconKind,
		string header)
		where TContent : class
	{
		builder.Register(context =>
		{
			var scope = context.Resolve<ILifetimeScope>();
			return new TabItemViewModel(iconKind, header, ContentFactory);

			(object, IDisposable) ContentFactory()
			{
				var contentScope = scope.BeginLifetimeScope(typeof(TContent));
				var content = contentScope.Resolve<TContent>();
				return (content, contentScope);
			}
		}).As<TabItemViewModel>();
	}

	private static void AddDataSetType<TTag, TAsset, TTagsEditor>(this ContainerBuilder builder, string name)
		where TAsset : class
		where TTagsEditor : class
	{
		builder.Register(context =>
		{
			var dataSetFactory = context.Resolve<Factory<DataSet<TTag, TAsset>>>();
			var tagsEditorFactory = context.Resolve<Func<TTagsEditor>>();

			return new DataSetTypeViewModel(name, (Factory<DataSet<Tag, Asset>>)dataSetFactory, (Func<TagsEditorDataContext>)tagsEditorFactory);
		});
	}
}