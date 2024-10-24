﻿using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserTag : ItemTag, TagsHolder
{
	Tag TagsHolder.CreateTag(string name) => CreateKeyPoint(name);

	public abstract override IReadOnlyCollection<PoserItem> Items { get; }
	public abstract IReadOnlyCollection<KeyPointTag> KeyPoints { get; }

	public abstract KeyPointTag CreateKeyPoint(string name);

	protected PoserTag(string name, IEnumerable<Tag> siblings) : base(name, siblings)
	{
	}
}