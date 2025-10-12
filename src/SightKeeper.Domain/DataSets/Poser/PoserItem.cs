using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserItem : DetectorItem
{
	new PoserTag Tag { get; set; }
	IReadOnlyList<KeyPoint> KeyPoints { get; }
	KeyPoint MakeKeyPoint(Tag tag);
	void DeleteKeyPoint(KeyPoint keyPoint);
	void ClearKeyPoints();

	Tag DetectorItem.Tag
	{
		get => Tag;
		set => Tag = (PoserTag)value;
	}
}