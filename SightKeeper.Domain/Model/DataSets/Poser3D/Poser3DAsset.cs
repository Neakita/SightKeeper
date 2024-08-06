using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DAsset : ItemsAsset<Poser3DItem>
{
	public override Poser3DScreenshot Screenshot { get; }
	public override Poser3DAssetsLibrary Library { get; }
	public override Poser3DDataSet DataSet => Library.DataSet;

	public Poser3DItem CreateItem(
		Poser3DTag tag,
		Bounding bounding,
		IReadOnlyCollection<(Vector2<double>, bool)> keyPoints,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties)
	{
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		var taggedKeyPoints = keyPoints.Zip(tag.KeyPoints).Select(t => new KeyPoint3D(t.First.Item1, t.Second, t.First.Item2)).ToList();
		Poser3DItem item = new(tag, bounding, taggedKeyPoints, numericProperties, booleanProperties);
		tag.AddItem(item);
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

	internal Poser3DAsset(Poser3DScreenshot screenshot, Poser3DAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}