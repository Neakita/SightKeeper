using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSets.Screenshots.Assets.Detector;

public sealed class DetectorItem : ObservableObject
{
	public Id Id { get; private set; }
	public Asset Asset { get; private set; }

	public ItemClass ItemClass
	{
		get => _itemClass;
		set => SetProperty(ref _itemClass, value);
	}

	public Bounding Bounding { get; private set; }
	
	internal DetectorItem(Asset asset, ItemClass itemClass, Bounding bounding)
	{
		Asset = asset;
		_itemClass = itemClass;
		Bounding = bounding;
	}
	
	private DetectorItem()
	{
		Asset = null!;
		_itemClass = null!;
		Bounding = null!;
	}
	
	private ItemClass _itemClass;
}