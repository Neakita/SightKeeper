using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class InMemoryTag : Tag
{
	public string Name { get; set; } = string.Empty;
	public uint Color { get; set; }
	public required TagsContainer<Tag> Owner { get; init; }
	public IReadOnlyCollection<TagUser> Users => _users;

	public void AddUser(TagUser user)
	{
		bool isAdded = _users.Add(user);
		Guard.IsTrue(isAdded);
	}

	public void RemoveUser(TagUser user)
	{
		bool isRemoved = _users.Remove(user);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<TagUser> _users = new();
}