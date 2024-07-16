using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Tests.Profiles.Behaviours;

public sealed class FakeAction : Action
{
	public override void Perform()
	{
	}
}