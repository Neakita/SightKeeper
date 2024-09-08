using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Tests.Profiles.Behaviors;

public sealed class FakeAction : Action
{
	public override void Perform()
	{
	}
}