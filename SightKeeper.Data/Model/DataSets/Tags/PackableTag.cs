using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="DomainTag"/>
/// </summary>
[MemoryPackable]
internal partial class PackableTag : Tag
{
	public string Name { get; set; } = string.Empty;
	public uint Color { get; set; }

	[MemoryPackIgnore]
	public TagsContainer<Tag> Owner
	{
		get
		{
			Guard.IsNotNull(_owner);
			return _owner;
		}
		set => _owner = value;
	}

	[MemoryPackIgnore] public IReadOnlyCollection<TagUser> Users => _users;

	internal void AddUser(TagUser user)
	{
		bool isAdded = _users.Add(user);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveUser(TagUser user)
	{
		bool isRemoved = _users.Remove(user);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<TagUser> _users = new();
	private TagsContainer<Tag>? _owner;
}