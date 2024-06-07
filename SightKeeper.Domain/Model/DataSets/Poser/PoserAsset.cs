namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserAsset : ItemsAsset<PoserItem>
{
	public PoserItem CreateItem(Tag tag, Bounding bounding, IEnumerable<KeyPoint> keyPoints)
	{
		PoserItem item = new(tag, bounding, keyPoints);
		AddItem(item);
		return item;
	}
	
	internal PoserAsset(Screenshot screenshot) : base(screenshot)
	{
	}
}