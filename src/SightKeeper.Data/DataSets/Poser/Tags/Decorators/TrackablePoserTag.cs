using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class TrackablePoserTag(PoserTag inner, ChangeListener listener) : PoserTag, Decorator<PoserTag>
{
	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
	public PoserTag Inner => inner;

	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			listener.SetDataChanged();
		}
	}

	public uint Color
	{
		get => inner.Color;
		set
		{
			inner.Color = value;
			listener.SetDataChanged();
		}
	}

	public Tag CreateKeyPointTag(string name)
	{
		var tag = inner.CreateKeyPointTag(name);
		listener.SetDataChanged();
		return tag;
	}

	public void DeleteKeyPointTagAt(int index)
	{
		inner.DeleteKeyPointTagAt(index);
		listener.SetDataChanged();
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		inner.DeleteKeyPointTag(tag);
		listener.SetDataChanged();
	}
}