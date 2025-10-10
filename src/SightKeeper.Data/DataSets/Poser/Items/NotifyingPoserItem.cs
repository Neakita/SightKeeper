using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class NotifyingPoserItem(PoserItem inner) : PoserItem, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			inner.Bounding = value;
			OnPropertyChanged();
		}
	}

	public PoserTag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value; 
			OnPropertyChanged();
		}
	}

	public IReadOnlyCollection<KeyPoint> KeyPoints => inner.KeyPoints;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		return inner.MakeKeyPoint(tag);
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		inner.DeleteKeyPoint(keyPoint);
	}

	public void ClearKeyPoints()
	{
		inner.ClearKeyPoints();
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}