using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class TrackableDetectorItem(StorableDetectorItem inner, ChangeListener listener) : StorableDetectorItem
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			inner.Bounding = value;
			listener.SetDataChanged();
		}
	}

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			listener.SetDataChanged();
		}
	}

	public StorableDetectorItem Innermost => inner.Innermost;
}