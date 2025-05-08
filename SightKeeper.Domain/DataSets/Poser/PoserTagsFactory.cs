using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

internal sealed class PoserTagsFactory : TagsFactory<PoserTag>
{
	public PoserTagsFactory(TagsUsageProvider tagsUsageProvider)
	{
		_tagsUsageProvider = tagsUsageProvider;
	}

	public override PoserTag CreateTag(TagsContainer<Tag> owner, string name)
	{
		return new PoserTag(owner, name, _tagsUsageProvider);
	}

	private readonly TagsUsageProvider _tagsUsageProvider;
}