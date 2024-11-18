using Autofac;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.Games;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Dialogs;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		BinarySerializationBootstrapper.Setup(builder);
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
		builder.RegisterType<DialogManager>().SingleInstance();
		builder.RegisterGeneric(typeof(ObservableRepository<>)).SingleInstance();
		builder.RegisterType<DataSetCreator>();
		builder.RegisterType<SharpHookScreenBoundsProvider>().As<ScreenBoundsProvider>();
		builder.RegisterType<Screenshotter<Bgra32>>().As<Screenshotter>();
		builder.RegisterType<BufferedScreenshotsSaver<Bgra32>>().As<ScreenshotsSaver<Bgra32>>().As<PendingScreenshotsCountReporter>().SingleInstance();
		builder.RegisterType<Bgra32ToRgba32PixelConverter>().As<PixelConverter<Bgra32, Rgba32>>();
		builder.RegisterType<WriteableBitmapPool>().SingleInstance();
		builder.RegisterType<ScreenshotImageLoader>().SingleInstance();
	}

	public static void Initialize(IComponentContext context)
	{
		BinarySerializationBootstrapper.Initialize(context);
	}
}