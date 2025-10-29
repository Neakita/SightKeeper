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
		
		builder.AddCondaLocator();

		builder.Register(context =>
		{
			var commandRunner = context.Resolve<CommandRunner>();
			var condaLocator = context.Resolve<CondaLocator>();
			commandRunner = new WindowsCondaCommandRunner(commandRunner, condaLocator);
			return new StatelessCondaEnvironmentManager(commandRunner);
		}).As<CondaEnvironmentManager>();

		builder.Register(_ =>
		{
			CommandRunner commandRunner = new ArgumentCommandRunner("cmd.exe");
			commandRunner = new WindowsArgumentCarryCommandRunner(commandRunner);
			return commandRunner;
		}).As<CommandRunner>();
	}

	private static void AddCondaLocator(this ContainerBuilder builder)
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