using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAsset : ItemsAsset<Poser3DItem>, AssetsFactory<Poser3DAsset>, AssetsDestroyer<Poser3DAsset>
{
	public static Poser3DAsset Create(Screenshot<Poser3DAsset> screenshot)
	{
		Poser3DAsset asset = new(screenshot, (AssetsLibrary<Poser3DAsset>)screenshot.DataSet.Assets);
		screenshot.SetAsset(asset);
		return asset;
	}

	public static void Destroy(Poser3DAsset asset)
	{
		asset.Screenshot.SetAsset(null);
		foreach (var item in asset.Items)
			item.Tag.RemoveItem(item);
	}

	public override Screenshot<Poser3DAsset> Screenshot { get; }
	public override AssetsLibrary<Poser3DAsset> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	public Poser3DItem CreateItem(
		Poser3DTag tag,
		Bounding bounding,
		IReadOnlyCollection<KeyPoint3D> keyPoints,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties)
	{
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		Poser3DItem item = new(tag, bounding, keyPoints, numericProperties, booleanProperties, this);
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

	internal Poser3DAsset(Screenshot<Poser3DAsset> screenshot, AssetsLibrary<Poser3DAsset> library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}