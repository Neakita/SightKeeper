using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class PassiveScalingOptionsConverter
{
	public static SerializablePassiveScalingOptions? Convert(PassiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			ConstantScalingOptions constantScalingOptions => new SerializableConstantScalingOptions(constantScalingOptions),
			IterativeScalingOptions iterativeScalingOptions => new SerializableIterativeScalingOptions(iterativeScalingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}

	public static PassiveScalingOptions? ConvertBack(SerializablePassiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			SerializableConstantScalingOptions constantScalingOptions => new ConstantScalingOptions
			{
				Factor = constantScalingOptions.Factor
			},
			SerializableIterativeScalingOptions iterativeScalingOptions => new IterativeScalingOptions
			{
				MinimumScaling = iterativeScalingOptions.MinimumScale,
				MaximumScaling = iterativeScalingOptions.MaximumScale
			},
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}