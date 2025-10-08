using Autofac;
using SightKeeper.Application.Linux.X11;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux;

public static class ContainerBuilderLinuxServicesExtensions
{
	public static void AddLinuxServices(this ContainerBuilder builder)
	{
		builder.RegisterType<SustainableScreenCapturer<Bgra32, X11ScreenCapturer>>()
			.As<ScreenCapturer<Bgra32>>();
	}
}