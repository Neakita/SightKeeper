using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class BehavioursConverter
{
	public static SerializableBehaviour Convert(Behaviour behaviour, ConversionSession session)
	{
		return behaviour switch
		{
			AimAssistBehaviour aimAssistBehaviour => AimAssistBehavioursConverter.Convert(aimAssistBehaviour, session),
			AimBehaviour aimBehaviour => AimBehaviourConverter.Convert(aimBehaviour, session),
			TriggerBehaviour triggerBehaviour => TriggerBehaviourConverter.Convert(triggerBehaviour, session),
			_ => throw new ArgumentOutOfRangeException(nameof(behaviour))
		};
	}
}