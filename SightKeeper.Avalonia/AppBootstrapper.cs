using MemoryPack;

namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static void Setup(Composition composition)
	{
		MemoryPackFormatterProvider.Register(composition.AppDataFormatter);
		composition.AppDataAccess.Load();
		composition.PeriodicAppDataSaver.Start();
	}
}