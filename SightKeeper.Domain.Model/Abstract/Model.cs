using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Model : IModel
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
		if (!CanCreateItemClass(name, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		ItemClass newItemClass = new(name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}

	public bool CanCreateItemClass(string newItemClassName, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_itemClasses.Any(itemClass => itemClass.Name == newItemClassName))
			message = $"Item class with name \"{newItemClassName}\" already exists";
		return message == null;
	}

	public void AddItemClass(ItemClass itemClass)
	{
		if (!CanAddItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Add(itemClass);
	}

	public bool CanAddItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_itemClasses.Contains(itemClass)) message = "Item class already added";
		else if (_itemClasses.Any(i => i.Name == itemClass.Name))
			message = $"Item class with name \"{itemClass.Name}\" already exists";
		return message == null;
	}
	
	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}

	public abstract bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message);
	
	private readonly List<ItemClass> _itemClasses;

	#endregion

	#region Screenshots
	
	public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;

	public void AddScreenshot(Screenshot screenshot)
	{
		if (!CanAddScreenshot(screenshot, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_screenshots.Add(screenshot);
	}
	
	public abstract bool CanAddScreenshot(Screenshot screenshot, [NotNullWhen(false)] out string? message);
	
	public void DeleteScreenshot(Screenshot screenshot)
	{
		if (!_screenshots.Remove(screenshot))
			ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
	}
	
	private readonly List<Screenshot> _screenshots;

	#endregion

	#region Weights
	
	public IReadOnlyCollection<ModelWeights> Weights => _weights;

	public void AddWeights(ModelWeights weights)
	{
		if (_weights.Contains(weights)) ThrowHelper.ThrowArgumentException("Weights already added");
		_weights.Add(weights);
	}
	
	private readonly List<ModelWeights> _weights;

	#endregion
	
	protected Model(string name) : this(name, new Resolution())
	{
	}

	protected Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		_weights = new List<ModelWeights>();
		_screenshots = new List<Screenshot>();
	}

	protected Model(string name, string description)
	{
		Name = name;
		Description = description;
		_resolution = null!;
		_itemClasses = null!;
		_weights = null!;
		_screenshots = null!;
	}
	
	private ModelConfig? _config;
}