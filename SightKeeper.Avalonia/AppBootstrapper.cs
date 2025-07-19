using Serilog;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static void Setup(Composition composition)
	{
		SetupLogger();
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug()
			.CreateLogger();
	}
}