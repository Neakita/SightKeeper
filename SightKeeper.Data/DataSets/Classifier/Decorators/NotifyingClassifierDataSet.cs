using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier.Decorators;

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

	public StorableTagsOwner<StorableTag> TagsLibrary => inner.TagsLibrary;
	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary => inner.AssetsLibrary;
	public StorableWeightsLibrary WeightsLibrary => inner.WeightsLibrary;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}