using SightKeeper.Data.Binary.Profiles.Modules.Walking;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class ActiveWalkingOptionsConverter
{
	public static SerializableActiveWalkingOptions? Convert(ActiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			TrackingWalkingOptions trackingWalkingOptions => new SerializableTrackingWalkingOptions(trackingWalkingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}