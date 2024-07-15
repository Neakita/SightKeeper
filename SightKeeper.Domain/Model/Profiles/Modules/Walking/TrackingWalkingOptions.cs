using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Walking;

public sealed class TrackingWalkingOptions : ActiveWalkingOptions
{
	public Vector2<float> MaximumOffset
	{
		get => _maximumOffset;
		set
		{
			Guard.IsGreaterThan(value.X, 0);
			Guard.IsGreaterThan(value.Y, 0);
			_maximumOffset = value;
		}
	}

	private Vector2<float> _maximumOffset;
}