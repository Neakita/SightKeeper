using MemoryPack;
using Serilog;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static void Setup(Composition composition)
	{
		SetupLogger();
		MemoryPackFormatterProvider.Register(composition.AppDataFormatter);
		composition.AppDataAccess.Load();
		composition.PeriodicAppDataSaver.Start();
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug()
			.CreateLogger();
	}
}