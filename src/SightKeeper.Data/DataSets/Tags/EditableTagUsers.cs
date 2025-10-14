using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal interface EditableTagUsers
{
	void AddUser(TagUser user);
	void RemoveUser(TagUser user);
}