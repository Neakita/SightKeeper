using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAsset : ItemsAsset<PoserItem>
{
	public override PoserScreenshot Screenshot { get; }
	public override PoserAssetsLibrary Library { get; }
	public override PoserDataSet DataSet => Library.DataSet;

	public PoserItem CreateItem(PoserTag tag, Bounding bounding, IReadOnlyCollection<Vector2<double>> keyPoints)
	{
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		var taggedKeyPoints = keyPoints.Zip(tag.KeyPoints).Select(t => new KeyPoint(t.First, t.Second)).ToList();
		PoserItem item = new(tag, bounding, taggedKeyPoints);
		tag.AddItem(item);
		AddItem(item);
		return item;
	}

	public override void DeleteItem(PoserItem item)
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

	internal PoserAsset(PoserScreenshot screenshot, PoserAssetsLibrary library)
	{
		Screenshot = screenshot;
		Library = library;
	}
}