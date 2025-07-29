using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

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

	public AssetsOwner<StorableClassifierAsset> AssetsLibrary => inner.AssetsLibrary;

	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}