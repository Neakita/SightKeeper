using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserItem : AssetItem
{
	new PoserTag Tag { get; set; }
	Tag AssetItem.Tag => Tag;
	IReadOnlyCollection<KeyPoint> KeyPoints { get; }
	KeyPoint MakeKeyPoint(Tag tag);
	void DeleteKeyPoint(KeyPoint keyPoint);
	void ClearKeyPoints();
}