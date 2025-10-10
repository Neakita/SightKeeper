﻿using Autofac;
using Avalonia.Platform;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Avalonia.Misc;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Infrastructure;

internal static class PresentationServicesExtensions
{
	public static void AddPresentationServices(this ContainerBuilder builder)
	{
		builder.RegisterType<DialogManager>()
			.SingleInstance();

		builder.Register(context =>
		{
			var bitmapPool = context.Resolve<WriteableBitmapPool>();
			var imageLoader = context.Resolve<ImageLoader<Rgba32>>();
			return new WriteableBitmapImageLoader<Rgba32>(bitmapPool, PixelFormat.Rgb32, imageLoader);
		}).As<WriteableBitmapImageLoader>();

		builder.RegisterType<WriteableBitmapPool>()
			.SingleInstance();

		builder.RegisterType<ObservableSharpHookGesture>()
			.SingleInstance();

		builder.RegisterType<ImageSetViewModelsObservableListRepository>();
		builder.RegisterType<TagSelectionProvider>();
		builder.RegisterType<DataSetViewModelsObservableListRepository>();
		builder.RegisterType<ToolingViewModelFactory>();
		builder.RegisterType<DrawerItemsFactory>();
	}

	public static void AddCommands(this ContainerBuilder builder)
	{
		builder.RegisterType<CreateImageSetCommand>();
		builder.RegisterType<EditImageSetCommand>();
		builder.RegisterType<DeleteImageSetCommand>();
		builder.RegisterType<CreateDataSetCommand>();
		builder.RegisterType<ImportDataSetCommand>();
		builder.RegisterType<EditDataSetCommand>();
		builder.RegisterType<ExportDataSetCommand>();
		builder.RegisterType<DeleteDataSetCommand>();
		builder.RegisterType<DeleteSelectedImageCommand>();
		builder.RegisterType<DeleteSelectedAssetCommand>();
	}
}