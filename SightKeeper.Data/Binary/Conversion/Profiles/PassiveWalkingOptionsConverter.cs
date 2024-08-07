using IterativeWalkingOptions = SightKeeper.Data.Binary.Profiles.Modules.Walking.IterativeWalkingOptions;
using PassiveWalkingOptions = SightKeeper.Data.Binary.Profiles.Modules.Walking.PassiveWalkingOptions;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class PassiveWalkingOptionsConverter
{
	public static PassiveWalkingOptions? Convert(Domain.Model.Profiles.Modules.Walking.PassiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			Domain.Model.Profiles.Modules.Walking.IterativeWalkingOptions iterativeWalkingOptions => new IterativeWalkingOptions(iterativeWalkingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}

	public static Domain.Model.Profiles.Modules.Walking.PassiveWalkingOptions? ConvertBack(PassiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			IterativeWalkingOptions iterativeWalkingOptions => new Domain.Model.Profiles.Modules.Walking.IterativeWalkingOptions
			{
				OffsetStep = iterativeWalkingOptions.OffsetStep,
				MaximumSteps = iterativeWalkingOptions.MaximumSteps
			},
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}