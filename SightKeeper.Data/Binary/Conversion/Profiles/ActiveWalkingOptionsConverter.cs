using ActiveWalkingOptions = SightKeeper.Data.Binary.Profiles.Modules.Walking.ActiveWalkingOptions;
using TrackingWalkingOptions = SightKeeper.Data.Binary.Profiles.Modules.Walking.TrackingWalkingOptions;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class ActiveWalkingOptionsConverter
{
	public static ActiveWalkingOptions? Convert(Domain.Model.Profiles.Modules.Walking.ActiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			Domain.Model.Profiles.Modules.Walking.TrackingWalkingOptions trackingWalkingOptions => new TrackingWalkingOptions(trackingWalkingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}

	public static Domain.Model.Profiles.Modules.Walking.ActiveWalkingOptions? ConvertBack(ActiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			TrackingWalkingOptions trackingWalkingOptions => new Domain.Model.Profiles.Modules.Walking.TrackingWalkingOptions
			{
				MaximumOffset = trackingWalkingOptions.MaximumOffset
			},
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}