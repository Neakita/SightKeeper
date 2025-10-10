using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints.Decorators;

internal sealed class LockingKeyPoint(KeyPoint inner, Lock editingLock) : KeyPoint
{
	public Tag Tag => inner.Tag;

	public Vector2<double> Position
	{
		get => inner.Position;
		set
		{
			lock (editingLock)
				inner.Position = value;
		}
	}
}