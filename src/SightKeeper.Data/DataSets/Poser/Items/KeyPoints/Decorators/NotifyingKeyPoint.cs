using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.KeyPoints.Decorators;

internal sealed class NotifyingKeyPoint(KeyPoint inner) : KeyPoint, Decorator<KeyPoint>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public Tag Tag => inner.Tag;
	public KeyPoint Inner => inner;

	public Vector2<double> Position
	{
		get => inner.Position;
		set
		{
			inner.Position = value;
			OnPropertyChanged();
		}
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}