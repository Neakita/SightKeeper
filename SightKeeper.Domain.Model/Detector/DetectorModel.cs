using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorModel : Abstract.Model
{
	public ICollection<DetectorAsset> Assets { get; set; }

	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
		Assets = new List<DetectorAsset>();
	}

	public override bool GetCanChangeResolution([NotNullWhen(false)] out string? errorMessage)
	{
		errorMessage = null;
		var hasScreenshots = Screenshots.Any();
		var hasAssets = Assets.Any();
		if (hasScreenshots && hasAssets)
			errorMessage = "Cannot change resolution of model with screenshots and assets";
		else if (hasAssets)
			errorMessage = "Cannot change resolution of model with assets";
		else if (hasScreenshots)
			errorMessage = "Cannot change resolution of model with screenshots";
		return errorMessage == null;
	}
	
	private DetectorModel(string name, string description) : base(name, description)
	{
		Assets = null!;
	}
}