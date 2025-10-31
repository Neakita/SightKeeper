using Autofac;
using SightKeeper.Application.Linux.X11;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux;

public static class LinuxServicesExtensions
{
	public static void RegisterLinuxServices(this ContainerBuilder builder)
	{
		builder.RegisterType<X11ScreenCapturer>()
			.As<ScreenCapturer<Bgra32>>();

		builder.RegisterCondaLocator();

		builder.RegisterCommandRunner();

		builder.RegisterType<BashCondaCommandRunner>()
			.Named<CommandRunner>("conda");

		builder.RegisterType<StatelessCondaEnvironmentManager>()
			.Parameter().KeyFilter<CommandRunner>("conda")
			.As<CondaEnvironmentManager>();
	}

	private static void RegisterCondaLocator(this ContainerBuilder builder)
	{
		var userDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		IReadOnlyCollection<string> possiblePaths =
		[
			Path.Combine(userDirectoryPath, "miniconda3"),
			Path.Combine(userDirectoryPath, "Programs", "miniconda3")
		];
		var condaLocator = new CondaLocator(possiblePaths);
		builder.RegisterInstance(condaLocator);
	}

	private static void RegisterCommandRunner(this ContainerBuilder builder)
	{
		CommandRunner commandRunner = new ArgumentCommandRunner("/bin/bash");
		commandRunner = new BashArgumentCarryCommandRunner(commandRunner);

		builder.RegisterInstance(commandRunner)
			.As<CommandRunner>();
	}
}