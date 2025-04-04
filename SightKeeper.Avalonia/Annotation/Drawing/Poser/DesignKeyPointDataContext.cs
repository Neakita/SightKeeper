using Avalonia.Media;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class DesignKeyPointDataContext : KeyPointDataContext
{
	public static DesignKeyPointDataContext Instance => new()
	{
		Name = "Head",
		Color = Color.FromRgb(0xF0, 0x22, 0x22)
	};

	public required string Name { get; init; }
	public Color Color { get; init; }
	public Vector2<double> Position { get; set; }
}