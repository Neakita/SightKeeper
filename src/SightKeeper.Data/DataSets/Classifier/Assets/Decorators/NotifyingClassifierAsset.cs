using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class NotifyingClassifierAsset(ClassifierAsset inner) : ClassifierAsset, Decorator<ClassifierAsset>, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
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

	public ClassifierAsset Inner => inner;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}