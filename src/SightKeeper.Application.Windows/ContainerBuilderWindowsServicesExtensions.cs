using Autofac;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Windows;

public static class ContainerBuilderWindowsServicesExtensions
{
	public static void AddWindowsServices(this ContainerBuilder builder)
	{
		builder.RegisterType<SustainableScreenCapturer<Bgra32, DX11ScreenCapturer>>()
			.As<ScreenCapturer<Bgra32>>();

		builder.RegisterType<StatelessWindowsCondaEnvironmentManager>()
			.As<CondaEnvironmentManager>();

		builder.Register(_ =>
		{
			CommandRunner commandRunner = new ArgumentCommandRunner("cmd.exe");
			commandRunner = new WindowsArgumentCarryCommandRunner(commandRunner);
			return commandRunner;
		}).As<CommandRunner>();
	}
}