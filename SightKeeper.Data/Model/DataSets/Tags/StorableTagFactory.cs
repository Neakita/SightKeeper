using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Model.DataSets.Tags;

public sealed class StorableTagFactory : TagFactory<Tag>
{
	public Tag CreateTag(string name)
	{
		throw new NotImplementedException();
	}
}