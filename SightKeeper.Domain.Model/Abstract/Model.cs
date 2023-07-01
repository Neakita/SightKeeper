using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Model : IModel
{
	public string Name { get; set; }
	public string Description { get; set; }
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

	public IReadOnlyCollection<ItemClass> ItemClasses => _itemClasses;

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

	public IReadOnlyCollection<ModelWeights> Weights => _weights;
	public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;


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
	
	public ItemClass CreateItemClass(string name)
	{
		if (!CanCreateItemClass(name, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		ItemClass newItemClass = new(name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}

	public void AddItemClass(ItemClass itemClass)
	{
		if (!CanAddItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Add(itemClass);
	}

	public void DeleteItemClass(ItemClass itemClass)
	{
		if (!CanDeleteItemClass(itemClass, out var message))
			ThrowHelper.ThrowInvalidOperationException(message);
		_itemClasses.Remove(itemClass);
	}
	
	public abstract bool CanChangeResolution([NotNullWhen(false)] out string? message);
	public abstract bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message);

	public bool CanCreateItemClass(string newItemClassName, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_itemClasses.Any(itemClass => itemClass.Name == newItemClassName))
			message = $"Item class with name \"{newItemClassName}\" already exists";
		return message == null;
	}

	public bool CanAddItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_itemClasses.Contains(itemClass)) message = "Item class already added";
		else if (_itemClasses.Any(i => i.Name == itemClass.Name))
			message = $"Item class with name \"{itemClass.Name}\" already exists";
		return message == null;
	}

	public void AddWeights(ModelWeights weights)
	{
		if (_weights.Contains(weights)) ThrowHelper.ThrowArgumentException("Weights already added");
		_weights.Add(weights);
	}

	public void AddScreenshot(Screenshot screenshot)
	{
		if (Screenshots.Contains(screenshot))
			ThrowHelper.ThrowInvalidOperationException("Screenshot already added");
		_screenshots.Add(screenshot);
	}
	
	public void DeleteScreenshot(Screenshot screenshot)
	{
		if (!_screenshots.Remove(screenshot))
			ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
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

	private readonly List<ItemClass> _itemClasses;
	private readonly List<ModelWeights> _weights;
	private readonly List<Screenshot> _screenshots;
	private Resolution _resolution;
	private ModelConfig? _config;
}