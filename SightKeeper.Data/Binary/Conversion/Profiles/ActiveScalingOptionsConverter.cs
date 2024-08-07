using ActiveScalingOptions = SightKeeper.Data.Binary.Profiles.Modules.Scaling.ActiveScalingOptions;
using ScalingOptions = SightKeeper.Data.Binary.Profiles.Modules.Scaling.ScalingOptions;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class ActiveScalingOptionsConverter
{
	public static ActiveScalingOptions? Convert(Domain.Model.Profiles.Modules.Scaling.ActiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			Domain.Model.Profiles.Modules.Scaling.AdaptiveScalingOptions adaptiveScalingOptions => new ScalingOptions(adaptiveScalingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}

	public static Domain.Model.Profiles.Modules.Scaling.ActiveScalingOptions? ConvertBack(ActiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			ScalingOptions adaptiveScalingOptions => new Domain.Model.Profiles.Modules.Scaling.AdaptiveScalingOptions
			{
				Margin = adaptiveScalingOptions.Margin,
				MaximumScaling = adaptiveScalingOptions.MaximumScaling
			},
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}