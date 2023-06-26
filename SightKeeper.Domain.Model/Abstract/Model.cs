using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

public abstract class Model : Entity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Resolution Resolution
	{
		get => _resolution;
		set
		{
			if (!GetCanChangeResolution(out var message))
				throw new InvalidOperationException(message);
			_resolution = value;
		}
	}
	public ICollection<ItemClass> ItemClasses { get; set; }
	public Game? Game { get; set; }
	public ModelConfig? Config { get; set; }
	public ICollection<ModelWeights> Weights { get; set; }


	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		_resolution = resolution;
		ItemClasses = new List<ItemClass>();
		Weights = new List<ModelWeights>();
	}

	protected abstract bool GetCanChangeResolution([NotNullWhen(false)] out string? errorMessage);

	protected Model(int id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		_resolution = null!;
		ItemClasses = null!;
		Weights = null!;
	}

	private Resolution _resolution;
}