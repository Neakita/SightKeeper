using Avalonia;
using Avalonia.Headless;
using SightKeeper.Avalonia.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace SightKeeper.Avalonia.Tests;

public class TestAppBuilder
{
	public static AppBuilder BuildAvaloniaApp() => AppBuilder
		.Configure<TestApp>()
		.UseHeadless(new AvaloniaHeadlessPlatformOptions());
}