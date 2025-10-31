using Autofac;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Windows;

public static class WindowsServicesExtensions
{
	public static void RegisterWindowsServices(this ContainerBuilder builder)
	{
		builder.RegisterType<SustainableScreenCapturer<Bgra32, DX11ScreenCapturer>>()
			.As<ScreenCapturer<Bgra32>>();

		builder.RegisterCondaLocator();

		builder.Register(_ =>
		{
			CommandRunner commandRunner = new ArgumentCommandRunner("cmd.exe");
			commandRunner = new WindowsArgumentCarryCommandRunner(commandRunner);
			return commandRunner;
		}).As<CommandRunner>();

		builder.RegisterType<WindowsCondaCommandRunner>()
			.Named<CommandRunner>("conda");

		builder.RegisterType<StatelessCondaEnvironmentManager>()
			.WithParameter((info, _) => info.Position == 0, (_, context) => context.ResolveNamed<CommandRunner>("conda"))
			.As<CondaEnvironmentManager>();
	}

	private static void RegisterCondaLocator(this ContainerBuilder builder)
	{
		var userDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		IReadOnlyCollection<string> possiblePaths =
		[
			Path.Combine(userDirectoryPath, "Miniconda3")
		];
		var condaLocator = new CondaLocator(possiblePaths);
		builder.RegisterInstance(condaLocator);
	}
}