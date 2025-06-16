using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

internal sealed class PoserTagsFactory : TagsFactory<PoserTag>
{
	public static PoserTagsFactory Instance { get; } = new();

	public override PoserTag CreateTag(TagsContainer<DomainTag> owner, string name)
	{
		return new PoserTag(owner, name);
	}
}