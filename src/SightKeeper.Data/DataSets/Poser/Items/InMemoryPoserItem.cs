using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Poser.Items.KeyPoints;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class InMemoryPoserItem(PoserTag tag, KeyPointFactory keyPointFactory) : PoserItem
{
	public Bounding Bounding { get; set; }
	public PoserTag Tag { get; set; } = tag;
	public IReadOnlyCollection<KeyPoint> KeyPoints => _keyPoints;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		var keyPoint = keyPointFactory.CreateKeyPoint(tag);
		_keyPoints.Add(keyPoint);
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		bool isRemoved = _keyPoints.Remove(keyPoint);
		Guard.IsTrue(isRemoved);
	}

	public void ClearKeyPoints()
	{
		_keyPoints.Clear();
	}

	private List<KeyPoint> _keyPoints = new();
}