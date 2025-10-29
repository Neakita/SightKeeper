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

		builder.Register(context =>
		{
			var commandRunner = context.Resolve<CommandRunner>();
			commandRunner = new BashCondaCommandRunner(commandRunner);
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
}