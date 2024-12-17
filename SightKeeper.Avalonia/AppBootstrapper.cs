namespace SightKeeper.Avalonia;

internal static class AppBootstrapper
{
	public static void Setup(Composition composition)
	{
		composition.PeriodicAppDataSaver.Start();
	}
}