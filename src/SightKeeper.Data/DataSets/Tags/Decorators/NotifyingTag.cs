using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags.Decorators;

internal sealed class NotifyingTag(Tag inner) : Tag, Decorator<Tag>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

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

	public TagsContainer<Tag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public Tag Inner => inner;

	public void AddUser(TagUser user)
	{
		inner.AddUser(user);
	}

	public void RemoveUser(TagUser user)
	{
		inner.RemoveUser(user);
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}