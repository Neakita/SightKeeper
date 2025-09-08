using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class TrackableDetectorItem(DetectorItem inner, ChangeListener listener) : DetectorItem, Decorator<DetectorItem>
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

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			listener.SetDataChanged();
		}
	}

	public DetectorItem Inner => inner;
}