using CommunityToolkit.Diagnostics;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags;

internal sealed class InMemoryPoserTag(TagFactory<Tag> keyPointTagFactory) : PoserTag
{
	public string Name { get; set; } = string.Empty;
	public uint Color { get; set; }
	public IReadOnlyList<Tag> KeyPointTags => _keyPointTags;
	public required TagsContainer<PoserTag> Owner { get; init; }
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

	public Tag CreateKeyPointTag(string name)
	{
		var tag = keyPointTagFactory.CreateTag(name);
		_keyPointTags.Add(tag);
		return tag;
	}

	public void DeleteKeyPointTagAt(int index)
	{
		_keyPointTags.RemoveAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		bool isRemoved = _keyPointTags.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<TagUser> _users = new();
	private readonly List<Tag> _keyPointTags = new();
}