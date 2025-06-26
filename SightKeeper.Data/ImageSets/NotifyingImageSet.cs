using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class NotifyingImageSet(ImageSet inner) : ImageSet, INotifyPropertyChanged
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