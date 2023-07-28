using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorModel : Model
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

	public DetectorAsset MakeAssetFromScreenshot(Screenshot screenshot)
	{
		if (screenshot.Asset != null)
			ThrowHelper.ThrowArgumentException("Asset with same screenshot already exists");
		if (screenshot.Library is ModelScreenshotsLibrary modelLibrary)
		{
			if (modelLibrary.Model == this) ScreenshotsLibrary.DeleteScreenshot(screenshot);
			else ThrowHelper.ThrowArgumentException(nameof(screenshot), $"Screenshot is owned by {modelLibrary.Model}");
		}
		DetectorAsset asset = new(this, screenshot);
		_assets.Add(asset);
		return asset;
	}

	public void DeleteAsset(DetectorAsset asset)
	{
		if (!_assets.Remove(asset))
			ThrowHelper.ThrowArgumentException(nameof(asset), "Asset not found");
		asset.Screenshot.Asset = null;
	}

	public override bool CanChangeResolution([NotNullWhen(false)] out string? message)
	{
		message = null;
		var hasScreenshots = ScreenshotsLibrary.Screenshots.Any();
		var hasAssets = Assets.Any();
		if (hasScreenshots && hasAssets)
			message = "Cannot change resolution of model with screenshots and assets";
		else if (hasAssets)
			message = "Cannot change resolution of model with assets";
		else if (hasScreenshots)
			message = "Cannot change resolution of model with screenshots";
		return message == null;
	}

	protected override bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (itemClass.DetectorItems.Any())
			message = "Cannot delete item class with assets";
		return message == null;
	}

	private readonly List<DetectorAsset> _assets;

	private DetectorModel(string name, string description) : base(name, description)
	{
		_assets = null!;
	}
}