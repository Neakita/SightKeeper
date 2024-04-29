using System.Drawing.Imaging;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Windows;

public sealed class WindowsGameIconProvider : GameIconProvider
{
	public byte[]? GetIcon(Game game)
	{
		if (string.IsNullOrEmpty(game.ExecutablePath))
			return null;
		using var icon = Icon.ExtractAssociatedIcon(game.ExecutablePath);
		if (icon == null)
			return null;
		using MemoryStream stream = new();
		// icon.Save(stream) gives bad quality for some reason
		icon.ToBitmap().Save(stream, ImageFormat.Png);
		return stream.ToArray();
	}
}