using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public class DetectorModel : Abstract.Model
{
	public ICollection<Screenshot> Screenshots {  get; set; }
	public ICollection<DetectorAsset> Assets { get; set; }

	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
		Screenshots = new List<Screenshot>();
		Assets = new List<DetectorAsset>();
	}


	private DetectorModel(int id, string name, string description) : base(id, name, description)
	{
		Screenshots = null!;
		Assets = null!;
	}

	protected override bool GetCanChangeResolution([NotNullWhen(false)] out string? errorMessage)
	{
		var hasAssets = Assets.Any();
		var hasScreenshots = Screenshots.Any();
		if (hasAssets && hasScreenshots)
		{
			errorMessage = "Cannot change resolution of model with assets and screenshots";
			return false;
		}
		if (hasAssets)
		{
			errorMessage = "Cannot change resolution of model with assets";
			return false;
		}
		if (hasScreenshots)
		{
			errorMessage = "Cannot change resolution of model with screenshots";
			return false;
		}
		errorMessage = null;
		return true;
	}
}