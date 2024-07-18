using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class ActiveScalingOptionsConverter
{
	public static SerializableActiveScalingOptions? Convert(ActiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			AdaptiveScalingOptions adaptiveScalingOptions => new SerializableAdaptiveScalingOptions(adaptiveScalingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}

	public static ActiveScalingOptions? ConvertBack(SerializableActiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			SerializableAdaptiveScalingOptions adaptiveScalingOptions => new AdaptiveScalingOptions
			{
				Margin = adaptiveScalingOptions.Margin,
				MaximumScaling = adaptiveScalingOptions.MaximumScaling
			},
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}