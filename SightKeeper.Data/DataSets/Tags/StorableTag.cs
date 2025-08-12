using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

public interface StorableTag : Tag
{
	void AddUser(TagUser user);
	void RemoveUser(TagUser user);
}