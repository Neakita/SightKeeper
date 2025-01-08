using Avalonia.Markup.Xaml;

namespace SightKeeper.Avalonia.Tests;

internal sealed class TestApp : global::Avalonia.Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}
}