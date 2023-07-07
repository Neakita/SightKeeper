using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public abstract class Model
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }
	public ModelConfig? Config
	{
		get => _config;
		set
		{
			if (value != null && value.ModelType != this.GetDomainType())
				ThrowHelper.ThrowArgumentException(nameof(Config),
					$"Model type mismatch, model type must be equal to config model type, but model type is {this.GetDomainType()} and config type is {value.ModelType}");
			_config = value;
		}
	}

	#region Resolution

	public Resolution Resolution
	{
		get => _resolution;
		set
		{
			if (!CanChangeResolution(out var message))
				ThrowHelper.ThrowInvalidOperationException(message);
			_resolution = value;
		}
	}
	
	public abstract bool CanChangeResolution([NotNullWhen(false)] out string? message);

	private Resolution _resolution;

	#endregion

	#region ItemClasses
	
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	
	public ItemClass CreateItemClass(string name)
	{
		if (_itemClasses.Any(itemClass => itemClass.Name == name))
			ThrowHelper.ThrowArgumentException($"Item class with name \"{name}\" already exists");
		ItemClass newItemClass = new(name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}

	public void AddItemClass(ItemClass itemClass)
	{
		if (_itemClasses.Contains(itemClass)) ThrowHelper.ThrowArgumentException($"Item class \"{itemClass}\" already added");
		else if (_itemClasses.Any(i => i.Name == itemClass.Name))
			ThrowHelper.ThrowArgumentException($"Item class with name \"{itemClass.Name}\" already exists");
		_itemClasses.Add(itemClass);
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	private readonly List<ItemClass> _itemClasses;

	#endregion

	public ModelScreenshotsLibrary ScreenshotsLibrary { get; }
	public ModelWeightsLibrary WeightsLibrary { get; }
	
	protected Model(string name) : this(name, new Resolution())
	{
	}

	protected Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		WeightsLibrary = new ModelWeightsLibrary();
		ScreenshotsLibrary = new ModelScreenshotsLibrary(this);
	}

	protected Model(string name, string description)
	{
		Name = name;
		Description = description;
		_resolution = null!;
		_itemClasses = null!;
		WeightsLibrary = null!;
		ScreenshotsLibrary = null!;
	}
	
	protected abstract bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message);
	
	private ModelConfig? _config;
}