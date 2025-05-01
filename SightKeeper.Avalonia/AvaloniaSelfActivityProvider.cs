using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using SightKeeper.Application;

namespace SightKeeper.Avalonia;

internal sealed class AvaloniaSelfActivityProvider : SelfActivityProvider
{
	public bool IsOwnWindowActive =>
		((ClassicDesktopStyleApplicationLifetime)global::Avalonia.Application.Current!.ApplicationLifetime!).Windows.Single().IsActive;
}