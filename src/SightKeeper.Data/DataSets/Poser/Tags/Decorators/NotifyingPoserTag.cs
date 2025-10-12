using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class NotifyingPoserTag(PoserTag inner) : PoserTag, Decorator<PoserTag>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

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
			OnPropertyChanged();
		}
	}

	public uint Color
	{
		get => inner.Color;
		set
		{
			inner.Color = value;
			OnPropertyChanged();
		}
	}

	public void AddUser(TagUser user)
	{
		inner.AddUser(user);
	}

	public void RemoveUser(TagUser user)
	{
		inner.RemoveUser(user);
	}

	public Tag CreateKeyPointTag(string name)
	{
		return inner.CreateKeyPointTag(name);
	}

	public void DeleteKeyPointTagAt(int index)
	{
		inner.DeleteKeyPointTagAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		inner.DeleteKeyPointTag(tag);
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}