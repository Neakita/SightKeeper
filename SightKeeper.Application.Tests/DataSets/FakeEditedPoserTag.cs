using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.Tests.DataSets;

internal sealed class FakeEditedPoserTag : EditedPoserTagData
{
	public PoserTag Tag { get; }
	public string Name { get; }
	public uint Color { get; }
	public TagsChanges KeyPointTagsChanges { get; }

	public FakeEditedPoserTag(PoserTag tag, TagsChanges keyPointTagsChanges)
	{
		Tag = tag;
		Name = tag.Name;
		Color = default;
		KeyPointTagsChanges = keyPointTagsChanges;
	}
}