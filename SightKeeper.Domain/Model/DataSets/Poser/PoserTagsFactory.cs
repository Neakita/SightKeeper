using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

internal sealed class PoserTagsFactory : TagsFactory<PoserTag>
{
	public static PoserTagsFactory Instance { get; } = new();

	public override PoserTag CreateTag(TagsOwner owner, string name)
	{
		return new PoserTag(owner, name);
	}
}