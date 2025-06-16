using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeEditedPoserTag : EditedPoserTagData
{
	public DomainPoserTag Tag { get; }
	public string Name { get; }
	public uint Color { get; }
	public TagsChanges KeyPointTagsChanges { get; }

	public FakeEditedPoserTag(DomainPoserTag tag, TagsChanges keyPointTagsChanges)
	{
		Tag = tag;
		Name = tag.Name;
		Color = default;
		KeyPointTagsChanges = keyPointTagsChanges;
	}
}