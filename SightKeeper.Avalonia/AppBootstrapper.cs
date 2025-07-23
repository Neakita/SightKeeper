using MemoryPack;
using Serilog;
using SightKeeper.Avalonia.Compositions;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static Composition Setup()
	{
		Composition composition = new();
		Log.Verbose("Composition:\n{composition}", composition.ToString());
		SetupPersistence(composition);
		return composition;
	}

	private static void SetupPersistence(Composition composition)
	{
		MemoryPackFormatterProvider.Register(composition.ImageSetFormatter);
		composition.AppDataAccess.Load();
	}
}