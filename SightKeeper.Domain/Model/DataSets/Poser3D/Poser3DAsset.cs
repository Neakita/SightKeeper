using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAsset : ItemsAsset<Poser3DItem>
{
	public override Screenshot<Poser3DAsset> Screenshot { get; }
	public override Poser3DAssetsLibrary Library { get; }
	public override Poser3DDataSet DataSet => Library.DataSet;

	public Poser3DItem CreateItem(
		Poser3DTag tag,
		Bounding bounding,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties)
	{
		bounding.EnsureNormalized();
		Poser3DItem item = new(tag, bounding, numericProperties, booleanProperties, this);
		AddItem(item);
		return item;
	}

	public override void DeleteItem(Poser3DItem item)
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

	internal Poser3DAsset(Screenshot<Poser3DAsset> screenshot, Poser3DAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}