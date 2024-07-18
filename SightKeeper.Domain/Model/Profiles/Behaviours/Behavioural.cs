namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public interface Behavioural
{
	AimBehaviour SetAimBehaviour();
	AimAssistBehaviour SetAimAssistBehaviour();
	TriggerBehaviour SetTriggerBehaviour();
}