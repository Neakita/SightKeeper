using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.Saving.DataSets;

internal sealed class FakeTagFactory : TagFactory<Tag>
{
	public Tag CreateTag(string name)
	{
		return new InMemoryTag
		{
			Name = name,
			Owner = null!
		};
	}
}