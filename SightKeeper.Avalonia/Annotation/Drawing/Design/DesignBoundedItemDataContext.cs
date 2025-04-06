using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing.Design;

internal sealed class DesignBoundedItemDataContext : BoundedItemDataContext
{
	public static DesignBoundedItemDataContext Instance => new()
	{
		Name = "Cop",
		Color = Color.FromRgb(0xF0, 0x22, 0x22),
		Bounding = default
	};

	public required string Name { get; init; }
	public Color Color { get; init; }
	public Bounding Bounding { get; set; }
	public Vector2<double> Position => Bounding.Position;
}