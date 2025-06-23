using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

// Represents ready to modify, track/observer and saving ImageSet 
// I wanted to use series of decorators so each decorator does small part of the job,
// but I considered it will be not obvious to cast instance through some method and not language casting operators,
// so interface(s) should be implemented by single class
internal sealed class StorableImageSet(ImageSet inner) : ImageSet, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public string Name
	{
		get => inner.Name;
		set
		{
			if (value == Name)
				return;
			inner.Name = value;
			OnPropertyChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			if (value == Description)
				return;
			inner.Description = value;
			OnPropertyChanged();
		}
	}

	public IReadOnlyList<Image> Images => inner.Images;

	public Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		return inner.CreateImage(creationTimestamp, size);
	}

	public IReadOnlyList<Image> GetImagesRange(int index, int count)
	{
		// read can be performed directly from packable instance, not a big deal.
		return inner.GetImagesRange(index, count);
	}

	public int IndexOf(Image image)
	{
		return inner.IndexOf(image);
	}

	public void RemoveImageAt(int index)
	{
		inner.RemoveImageAt(index);
	}

	public void RemoveImagesRange(int index, int count)
	{
		inner.RemoveImagesRange(index, count);
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}