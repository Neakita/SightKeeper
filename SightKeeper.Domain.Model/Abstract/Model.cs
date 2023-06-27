using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
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
				ThrowHelper.ThrowInvalidOperationException(message);
			_resolution = value;
		}
	}
	public ICollection<ItemClass> ItemClasses { get; set; }
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
		ItemClasses = new List<ItemClass>();
		Weights = new List<ModelWeights>();
		Screenshots = new List<Screenshot>();
	}

	public abstract bool GetCanChangeResolution([NotNullWhen(false)] out string? errorMessage);

	protected Model(int id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		_resolution = null!;
		ItemClasses = null!;
		Weights = null!;
		Screenshots = null!;
	}

	private Resolution _resolution;
	private ModelConfig? _config;
}