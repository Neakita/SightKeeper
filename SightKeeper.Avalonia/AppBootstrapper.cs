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
		composition.FileSystemImageRepository.ClearUnassociatedImageFiles();
		composition.PeriodicDataSaver.Start();
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug()
			.CreateLogger();
	}
}