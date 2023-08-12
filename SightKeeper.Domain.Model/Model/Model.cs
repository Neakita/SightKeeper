using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public abstract class Model
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Game? Game { get; set; }

	#region Resolution

	public Resolution Resolution
	{
		get => _resolution;
		set
		{
			if (value.Equals(_resolution))
				return;
			if (!CanChangeResolution(out var message))
				ThrowHelper.ThrowInvalidOperationException(message);
			_resolution = value;
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

	private Resolution _resolution;

	#endregion

	#region ItemClasses
	
	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;
	
	public ItemClass CreateItemClass(string name)
	{
		if (_itemClasses.Any(itemClass => itemClass.Name == name))
			ThrowHelper.ThrowArgumentException($"Item class with name \"{name}\" already exists");
		ItemClass newItemClass = new(this, name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	private readonly List<ItemClass> _itemClasses;

	#endregion

	public ModelScreenshotsLibrary ScreenshotsLibrary { get; private set; }
	public ModelWeightsLibrary WeightsLibrary { get; private set; }

	public abstract bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message);
	
	public override string ToString() => Name;

	protected Model(string name) : this(name, new Resolution())
	{
	}

	protected Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		WeightsLibrary = new ModelWeightsLibrary(this);
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
	
	private ModelConfig? _config;
}