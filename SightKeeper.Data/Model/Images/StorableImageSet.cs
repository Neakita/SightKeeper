using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

// Represents ready to modify, track/observer and saving ImageSet 
// I wanted to use series of decorators so each decorator does small part of the job,
// but I considered it will be not obvious to cast instance through some method and not language casting operators,
// so interface(s) should be implemented by single class
internal sealed class StorableImageSet : ImageSet, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public string Name
	{
		get => Packable.Name;
		set
		{
			if (value == Name)
				return;
			_decorated.Name = value;
			OnPropertyChanged();
		}
	}

	public string Description
	{
		get => Packable.Description;
		set
		{
			if (value == Description)
				return;
			_decorated.Description = value;
			OnPropertyChanged();
		}
	}

	public IReadOnlyList<DomainImage> Images => _decorated.Images;

	// this property used only for saving.
	// No changes should be made through this instance as it isn't decorated with domain rules, locking and changes tracking.
	public PackableImageSet Packable { get; }

	public StorableImageSet(PackableImageSet packable, Lock editingLock, ChangeListener changeListener)
	{
		Packable = packable;
		_decorated = packable
			// tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
			// and then tracking decorator will send another notification.
			.WithTracking(changeListener)
			// locking of domain rules can be relatively computationally heavy,
			// for example when removing images range every image should be checked if it is used by some asset,
			// so locking appears only after domain rules validated.
			.WithLocking(editingLock)
			// images observing decorator can also be before domain rules,
			// but domain rules can often throw exceptions so placing observing decorator after domain rules will make stack trace a bit shorter
			.WithObservableImages()
			.WithDomainRules();
	}

	public DomainImage CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return _decorated.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<DomainImage> GetImagesRange(int index, int count)
	{
		// read can be performed directly from packable instance, not a big deal.
		return Packable.GetImagesRange(index, count);
	}

	public int IndexOf(DomainImage image)
	{
		return Packable.IndexOf(image);
	}

	public void RemoveImageAt(int index)
	{
		_decorated.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		_decorated.RemoveImagesRange(index, count);
	}

	private readonly ImageSet _decorated;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}