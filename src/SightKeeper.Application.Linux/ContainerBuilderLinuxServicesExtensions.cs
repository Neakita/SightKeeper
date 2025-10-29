using Autofac;
using SightKeeper.Application.Linux.X11;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Linux;

public static class ContainerBuilderLinuxServicesExtensions
{
	public static void AddLinuxServices(this ContainerBuilder builder)
	{
		builder.RegisterType<X11ScreenCapturer>()
			.As<ScreenCapturer<Bgra32>>();

		builder.AddCondaLocator();

		builder.Register(context =>
		{
			var commandRunner = context.Resolve<CommandRunner>();
			var condaLocator = context.Resolve<CondaLocator>();
			commandRunner = new BashCondaCommandRunner(commandRunner, condaLocator);
			return new StatelessCondaEnvironmentManager(commandRunner);
		}).As<CondaEnvironmentManager>();

		builder.Register(_ =>
			{
				CommandRunner commandRunner = new ArgumentCommandRunner("/bin/bash");
				commandRunner = new BashArgumentCarryCommandRunner(commandRunner);
				return commandRunner;
			})
			.As<CommandRunner>();
	}

	private static void AddCondaLocator(this ContainerBuilder builder)
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
}