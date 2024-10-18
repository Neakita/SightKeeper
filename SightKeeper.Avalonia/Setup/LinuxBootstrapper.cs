#if OS_LINUX
using Autofac;
using SightKeeper.Application.Linux.X11;
using SightKeeper.Application.Screenshotting;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Avalonia.Setup;

internal static class LinuxBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<X11ScreenCapture>().As<ScreenCapture<Bgra32>>();
	}
}
#endif