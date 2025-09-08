using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class LockingDetectorItem(DetectorItem inner, Lock editingLock) : DetectorItem, Decorator<DetectorItem>
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			lock (editingLock)
				inner.Bounding = value;
		}
	}

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			lock (editingLock)
				inner.Tag = value;
		}
	}

	public DetectorItem Inner => inner;
}