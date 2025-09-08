using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class NotifyingDetectorItem(DetectorItem inner) : DetectorItem, INotifyPropertyChanged, Decorator<DetectorItem>
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

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			OnPropertyChanged();
		}
	}

	public DetectorItem Inner => inner;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}