using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeEditedTagData : EditedTagData
{
	public Tag Tag { get; }
	public string Name { get; }
	public uint Color { get; }

	public FakeEditedTagData(Tag tag, string name)
	{
		Tag = tag;
		Name = name;
		Color = default;
	}
}