using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Model
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
				ThrowHelper.ThrowArgumentException(nameof(Config), "Model type mismatch");
			_config = value;
		}
	}

	public ICollection<ModelWeights> Weights { get; set; }
	public ICollection<Screenshot> Screenshots { get; set; }


	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		_itemClasses = new List<ItemClass>();
		Weights = new List<ModelWeights>();
		Screenshots = new List<Screenshot>();
	}

	private bool CanCreateItemClass(string newItemClassName, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (_itemClasses.Any(itemClass => itemClass.Name == newItemClassName))
			message = $"Item class with name \"{newItemClassName}\" already exists";
		return message == null;
	}
	
	public ItemClass CreateItemClass(string name)
	{
		if (!CanCreateItemClass(name, out var message))
			throw new InvalidOperationException(message);
		ItemClass newItemClass = new(name);
		_itemClasses.Add(newItemClass);
		return newItemClass;
	}
	
	public abstract bool CanChangeResolution([NotNullWhen(false)] out string? message);

	protected Model(string name, string description)
	{
		Name = name;
		Description = description;
		_resolution = null!;
		_itemClasses = null!;
		Weights = null!;
		Screenshots = null!;
	}

	private readonly List<ItemClass> _itemClasses;
	private Resolution _resolution;
	private ModelConfig? _config;
}