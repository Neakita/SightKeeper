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

	public override bool CanChangeResolution([NotNullWhen(false)] out string? message)
	{
		message = null;
		var hasScreenshots = Screenshots.Any();
		var hasAssets = Assets.Any();
		if (hasScreenshots && hasAssets)
			message = "Cannot change resolution of model with screenshots and assets";
		else if (hasAssets)
			message = "Cannot change resolution of model with assets";
		else if (hasScreenshots)
			message = "Cannot change resolution of model with screenshots";
		return message == null;
	}

	public override bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (Assets.Any(asset => asset.Items.Any(item => item.ItemClass == itemClass)))
			message = "Cannot delete item class with assets";
		return message == null;
	}

	private DetectorModel(string name, string description) : base(name, description)
	{
		Assets = null!;
	}
}