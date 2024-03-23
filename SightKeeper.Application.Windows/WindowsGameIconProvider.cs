using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Windows;

public sealed class WindowsGameIconProvider : GameIconProvider
{
	public byte[]? GetIcon(Game game)
	{
		if (game.ExecutablePath == null)
			return null;
		var icon = Icon.ExtractAssociatedIcon(game.ExecutablePath);
		if (icon == null)
			return null;
		MemoryStream stream = new();
		icon.Save(stream);
		return stream.ToArray();
	}
}