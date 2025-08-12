using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class NotifyingDetectorDataSet(StorableDetectorDataSet inner) : StorableDetectorDataSet, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public string Name
	{
		get => inner.Name;
		set
		{
			inner.Name = value;
			OnPropertyChanged();
		}
	}

	public string Description
	{
		get => inner.Description;
		set
		{
			inner.Description = value;
			OnPropertyChanged();
		}
	}

	public StorableDetectorDataSet Innermost => inner.Innermost;

	public StorableTagsOwner<StorableTag> TagsLibrary => inner.TagsLibrary;

	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary => inner.AssetsLibrary;

	public StorableWeightsLibrary WeightsLibrary => inner.WeightsLibrary;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}