using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets;

internal sealed class FakeTagFactory : TagFactory<StorableTag>
{
	public StorableTag CreateTag(string name)
	{
		return new InMemoryTag
		{
			Name = name,
			Owner = null!
		};
	}
}