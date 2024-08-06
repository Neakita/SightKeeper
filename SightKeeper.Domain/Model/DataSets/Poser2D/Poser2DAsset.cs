using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DAsset : ItemsAsset<Poser2DItem>
{
	public override Poser2DScreenshot Screenshot { get; }
	public override Poser2DAssetsLibrary Library { get; }
	public override Poser2DDataSet DataSet => Library.DataSet;

	public Poser2DItem CreateItem(Poser2DTag tag, Bounding bounding, IReadOnlyCollection<Vector2<double>> keyPoints, ImmutableList<double> properties)
	{
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		Guard.IsEqualTo(properties.Count, tag.Properties.Count);
		var taggedKeyPoints = keyPoints.Zip(tag.KeyPoints).Select(t => new KeyPoint2D(t.First, t.Second)).ToList();
		Poser2DItem item = new(tag, bounding, taggedKeyPoints, properties);
		tag.AddItem(item);
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

	internal Poser2DAsset(Poser2DScreenshot screenshot, Poser2DAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}