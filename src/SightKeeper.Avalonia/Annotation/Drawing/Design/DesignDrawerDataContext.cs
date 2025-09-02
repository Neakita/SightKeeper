using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing.Design;

internal sealed class DesignDrawerDataContext : DrawerDataContext
{
	public static DesignDrawerDataContext WithImage => new()
	{
		Image = new DesignImageDataContext("kfSample6.jpg"),
		Items =
		[
			new DesignBoundedItemDataContext
			{
				Name = "Bloat",
				Bounding = new Bounding(0.415, 0.36, 0.525, 0.7),
				Color = Color.FromRgb(0xF0, 0x22, 0x22)
			},
			new DesignKeyPointDataContext
			{
				Name = "Head",
				Color = Color.FromRgb(0xF0, 0x22, 0x22),
				Position = new Vector2<double>(0.45, 0.4)
			}
		],
		SelectedItem = null
	};

	public static DesignDrawerDataContext WithoutImage => new();

	public ImageDataContext? Image { get; init; }

	public IReadOnlyCollection<DrawerItemDataContext> Items { get; init; } =
		ReadOnlyObservableCollection<DrawerItemDataContext>.Empty;

	public BoundedItemDataContext? SelectedItem { get; set; }

	public BoundingDrawerDataContext BoundingDrawer => new DesignBoundingDrawerDataContext();
	public KeyPointDrawerDataContext KeyPointDrawer => new DesignKeyPointDrawerDataContext(false);
}