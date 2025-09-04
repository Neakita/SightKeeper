using System.ComponentModel;
using System.Runtime.CompilerServices;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector.Decorators;

internal sealed class NotifyingDetectorDataSet(DetectorDataSet inner) : DetectorDataSet, Decorator<DetectorDataSet>, INotifyPropertyChanged
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

	public TagsOwner<Tag> TagsLibrary => inner.TagsLibrary;
	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary => inner.AssetsLibrary;
	public WeightsLibrary WeightsLibrary => inner.WeightsLibrary;
	public DetectorDataSet Inner => inner;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}