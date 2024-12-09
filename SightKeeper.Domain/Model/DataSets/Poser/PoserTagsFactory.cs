using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

internal sealed class PoserTagsFactory : TagsFactory<PoserTag>
{
	public PoserTagsFactory(TagsUsageProvider tagsUsageProvider)
	{
		_tagsUsageProvider = tagsUsageProvider;
	}

	public override PoserTag CreateTag(TagsOwner owner, string name)
	{
		return new PoserTag(owner, name, _tagsUsageProvider);
	}

	private readonly TagsUsageProvider _tagsUsageProvider;
}