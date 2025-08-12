using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class LockingDetectorItem(StorableDetectorItem inner, Lock editingLock) : StorableDetectorItem
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

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			lock (editingLock)
				inner.Tag = value;
		}
	}

	public StorableDetectorItem Innermost => inner.Innermost;
}