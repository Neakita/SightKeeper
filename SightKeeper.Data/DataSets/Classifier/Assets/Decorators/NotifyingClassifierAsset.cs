using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class NotifyingClassifierAsset(StorableClassifierAsset inner) : StorableClassifierAsset, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public StorableImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			OnPropertyChanged();
		}
	}

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			OnPropertyChanged();
		}
	}

	public StorableClassifierAsset Innermost => inner.Innermost;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}