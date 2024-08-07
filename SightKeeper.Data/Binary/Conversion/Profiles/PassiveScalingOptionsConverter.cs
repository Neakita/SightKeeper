using ConstantScalingOptions = SightKeeper.Data.Binary.Profiles.Modules.Scaling.ConstantScalingOptions;
using IterativeScalingOptions = SightKeeper.Data.Binary.Profiles.Modules.Scaling.IterativeScalingOptions;
using PassiveScalingOptions = SightKeeper.Data.Binary.Profiles.Modules.Scaling.PassiveScalingOptions;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class PassiveScalingOptionsConverter
{
	public static PassiveScalingOptions? Convert(Domain.Model.Profiles.Modules.Scaling.PassiveScalingOptions? options)
	{
		return options switch
		{
			null => null,
			Domain.Model.Profiles.Modules.Scaling.ConstantScalingOptions constantScalingOptions => new ConstantScalingOptions(constantScalingOptions),
			Domain.Model.Profiles.Modules.Scaling.IterativeScalingOptions iterativeScalingOptions => new IterativeScalingOptions(iterativeScalingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}