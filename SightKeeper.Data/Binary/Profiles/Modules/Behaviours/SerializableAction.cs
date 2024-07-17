﻿using FlakeId;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

internal class SerializableAction
{
	public Id TagId { get; set; }

	public SerializableAction(Id tagId)
	{
		TagId = tagId;
	}
}