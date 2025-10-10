using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints.Decorators;

internal sealed class TrackableKeyPoint(KeyPoint inner, ChangeListener listener) : KeyPoint
{
	public Tag Tag => inner.Tag;

	public Vector2<double> Position
	{
		get => inner.Position;
		set
		{
			inner.Position = value;
			listener.SetDataChanged();
		}
	}
}