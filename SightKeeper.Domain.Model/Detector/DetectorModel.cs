using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorModel : Abstract.Model
{
	public IReadOnlyCollection<DetectorAsset> Assets => _assets;

	public DetectorModel(string name) : this(name, new Resolution())
	{
	}

	public DetectorModel(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorModel(string name, Resolution resolution) : base(name, resolution)
	{
		_assets = new List<DetectorAsset>();
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

	public override bool CanAddScreenshot(Screenshot screenshot, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (Screenshots.Contains(screenshot))
			message = "Screenshot already added";
		else if (Assets.Any(asset => asset.Screenshot == screenshot))
			message = "Screenshot already added via asset";
		return message == null;
	}

	public DetectorAsset MakeAssetFromScreenshot(Screenshot screenshot)
	{
		if (Assets.Any(asset => asset.Screenshot == screenshot))
			ThrowHelper.ThrowArgumentException("Asset with same screenshot already exists");
		DeleteScreenshot(screenshot);
		DetectorAsset asset = new(this, screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(DetectorAsset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
	}

	private readonly List<DetectorAsset> _assets;

	private DetectorModel(string name, string description) : base(name, description)
	{
		_assets = null!;
	}
}