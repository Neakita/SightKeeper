using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public interface Poser3DItem : PoserItem
{
	new IReadOnlyCollection<KeyPoint3D> KeyPoints { get; }
	new KeyPoint3D MakeKeyPoint(Tag tag);
	void DeleteKeyPoint(KeyPoint3D keyPoint);

	IReadOnlyCollection<KeyPoint> PoserItem.KeyPoints => KeyPoints;

	KeyPoint PoserItem.MakeKeyPoint(Tag tag)
	{
		return MakeKeyPoint(tag);
	}

	void PoserItem.DeleteKeyPoint(KeyPoint keyPoint)
	{
		DeleteKeyPoint((KeyPoint3D)keyPoint);
	}
}