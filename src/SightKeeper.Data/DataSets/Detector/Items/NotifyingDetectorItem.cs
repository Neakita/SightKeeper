using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class NotifyingDetectorItem(StorableDetectorItem inner) : StorableDetectorItem, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public Bounding Bounding
	{
		get => inner.Bounding;
		set => inner.Bounding = value;
	}

	public StorableTag Tag
	{
		get => inner.Tag;
		set => inner.Tag = value;
	}

	public StorableDetectorItem Innermost => inner.Innermost;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}