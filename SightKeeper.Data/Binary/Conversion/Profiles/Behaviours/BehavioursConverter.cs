using AimAssistBehaviour = SightKeeper.Domain.Model.Profiles.Behaviours.AimAssistBehaviour;
using AimBehaviour = SightKeeper.Domain.Model.Profiles.Behaviours.AimBehaviour;
using Behaviour = SightKeeper.Data.Binary.Profiles.Modules.Behaviours.Behaviour;
using TriggerBehaviour = SightKeeper.Domain.Model.Profiles.Behaviours.TriggerBehaviour;

namespace SightKeeper.Data.Binary.Conversion.Profiles.Behaviours;

internal static class BehavioursConverter
{
	public static Behaviour Convert(Domain.Model.Profiles.Behaviours.Behaviour behaviour, ConversionSession session)
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