using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAsset : ItemsAsset<PoserItem>
{
	public PoserAssetsLibrary Library { get; }
	public PoserDataSet DataSet => Library.DataSet;

	public PoserItem CreateItem(PoserTag tag, Bounding bounding, IReadOnlyCollection<KeyPoint> keyPoints)
	{
		Guard.IsTrue(DataSet.Tags.Contains(tag));
		Guard.IsEqualTo(keyPoints.Count, tag.KeyPoints.Count);
		PoserItem item = new(tag, bounding, keyPoints);
		AddItem(item);
		return item;
	}
	
	internal PoserAsset(Screenshot screenshot, PoserAssetsLibrary library) : base(screenshot)
	{
		Library = library;
	}
}