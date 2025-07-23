using MemoryPack;
using Serilog;
using SightKeeper.Avalonia.Compositions;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static Composition Setup()
	{
		SetupLogger();
		Composition composition = new();
		Log.Verbose("Composition:\n{composition}", composition.ToString());
		SetupPersistence(composition);
		return composition;
	}

	private static void SetupLogger()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Verbose()
			.WriteTo.Debug()
			.CreateLogger();
	}

	private static void SetupPersistence(Composition composition)
	{
		MemoryPackFormatterProvider.Register(composition.ImageSetFormatter);
		composition.AppDataAccess.Load();
	}
}