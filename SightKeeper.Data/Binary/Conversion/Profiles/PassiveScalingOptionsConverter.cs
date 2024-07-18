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
}