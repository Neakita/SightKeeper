using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class NotifyingClassifierDataSet(StorableClassifierDataSet inner) : StorableClassifierDataSet, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public string Name
	{
		get => inner.Name;
		set
		{
			if (Name == value)
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
			if (Description == value)
				return;
			inner.Description = value;
			OnPropertyChanged();
		}
	}

	public TagsOwner<Tag> TagsLibrary => inner.TagsLibrary;
	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary => inner.AssetsLibrary;
	public StorableWeightsLibrary WeightsLibrary => inner.WeightsLibrary;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}