using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.DataSet.Screenshots;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets;
using SightKeeper.Domain.Model.DataSet.Weights;

namespace SightKeeper.Domain.Model.DataSet;

public sealed class DataSet : ObservableObject
{
	public Id Id { get; private set; }

	public string Name
	{
		get => _name;
		set => SetProperty(ref _name, value);
	}

	public string Description
	{
		get => _description;
		set => SetProperty(ref _description, value);
	}

	public Game? Game
	{
		get => _game;
		set => SetProperty(ref _game, value);
	}

	public ScreenshotsLibrary ScreenshotsLibrary { get; private set; }
	public WeightsLibrary WeightsLibrary { get; private set; }

	#region Resolution

	public ushort Resolution
	{
		get => _resolution;
		set
		{
			if (value.Equals(_resolution))
				return;
			if (!CanChangeResolution(out var message))
				ThrowHelper.ThrowInvalidOperationException(message);
			SetProperty(ref _resolution, value);
		}
	}

	public bool CanChangeResolution([NotNullWhen(false)] out string? message)
	{
		if (ScreenshotsLibrary.HasAnyScreenshots)
			message = "Can't change resolution when there are screenshots";
		else
			message = null;
		return message == null;
	}

	private ushort _resolution;

	#endregion

	#region ItemClasses
	
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	
	public ItemClass CreateItemClass(string name, uint color)
	{
		if (_itemClasses.Any(itemClass => itemClass.Name == name))
			ThrowHelper.ThrowArgumentException($"Item class with name \"{name}\" already exists");
		ItemClass newItemClass = new(this, name, color);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	public bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_assets.Any(asset => asset.Items.Any(item => item.ItemClass == itemClass)))
			message = $"Cannot delete item class {itemClass} because he is used in some asset";
		return message == null;
	}
	
	private readonly ObservableCollection<ItemClass> _itemClasses;
	private readonly ObservableCollection<Asset> _assets;
	private string _name;
	private string _description;
	private Game? _game;

	#endregion
	
	public override string ToString() => Name;

	public DataSet(string name, ushort resolution = 320)
	{
		_name = name;
		_description = string.Empty;
		_resolution = resolution;
		_itemClasses = new ObservableCollection<ItemClass>();
		ScreenshotsLibrary = new ScreenshotsLibrary(this);
		WeightsLibrary = new WeightsLibrary(this);
		_assets = new ObservableCollection<Asset>();
	}

	private DataSet()
	{
		_name = null!;
		_description = null!;
		_itemClasses = null!;
		ScreenshotsLibrary = null!;
		WeightsLibrary = null!;
		_assets = null!;
	}
	
	#region Assets

	public IReadOnlyCollection<Asset> Assets => _assets;

	public Asset MakeAsset(Screenshot screenshot)
	{
		var asset = new Asset(this, screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(Asset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
		asset.ClearItems();
	}

	#endregion
}