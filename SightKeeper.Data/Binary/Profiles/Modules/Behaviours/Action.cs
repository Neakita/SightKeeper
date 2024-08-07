using FlakeId;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

internal class Action
{
	public Id TagId { get; set; }

	public Action(Id tagId)
	{
		TagId = tagId;
	}
}