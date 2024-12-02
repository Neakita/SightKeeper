using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DAsset : ItemsAsset<Poser2DItem>
{
	public override Screenshot<Poser2DAsset> Screenshot { get; }
	public override Poser2DAssetsLibrary Library { get; }
	public override Poser2DDataSet DataSet => Library.DataSet;

	public Poser2DItem CreateItem(Poser2DTag tag, Bounding bounding, ImmutableList<double> properties)
	{
		bounding.EnsureNormalized();
		Guard.IsEqualTo(properties.Count, tag.Properties.Count);
		Poser2DItem item = new(tag, bounding, properties, this);
		AddItem(item);
		return item;
	}

	public override void DeleteItem(Poser2DItem item)
	{
		base.DeleteItem(item);
		item.Tag.RemoveItem(item);
	}

	public override void ClearItems()
	{
		foreach (var item in Items)
			item.Tag.RemoveItem(item);
		base.ClearItems();
	}

	internal Poser2DAsset(Screenshot<Poser2DAsset> screenshot, Poser2DAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}