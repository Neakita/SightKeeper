using SightKeeper.Data.Binary.Profiles.Modules.Walking;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal static class PassiveWalkingOptionsConverter
{
	public static SerializablePassiveWalkingOptions? Convert(PassiveWalkingOptions? options)
	{
		return options switch
		{
			null => null,
			IterativeWalkingOptions iterativeWalkingOptions => new SerializableIterativeWalkingOptions(iterativeWalkingOptions),
			_ => throw new ArgumentOutOfRangeException(nameof(options))
		};
	}
}