using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorDataSet : DataSet
{
	public IReadOnlyCollection<DetectorAsset> Assets => _assets;

	public DetectorDataSet(string name) : this(name, new Resolution())
	{
	}

	public DetectorDataSet(string name, ushort width, ushort height) : this(name, new Resolution(width, height))
	{
	}

	public DetectorDataSet(string name, Resolution resolution) : base(name, resolution)
	{
		_assets = new List<DetectorAsset>();
	}

	public DetectorAsset MakeAsset(Screenshot screenshot)
	{
		if (screenshot.Asset != null)
			ThrowHelper.ThrowArgumentException("Asset with same screenshot already exists");
		if (screenshot.Library is ModelScreenshotsLibrary modelLibrary && modelLibrary.DataSet != this)
			ThrowHelper.ThrowArgumentException(nameof(screenshot), $"Screenshot is owned by different model \"{modelLibrary.DataSet}\"");
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

	public override bool CanDeleteItemClass(ItemClass itemClass, [NotNullWhen(false)] out string? message)
	{
		message = null;
		if (itemClass.DetectorItems.Any())
			message = "Cannot delete item class with assets";
		return message == null;
	}

	private readonly List<DetectorAsset> _assets;

	private DetectorDataSet()
	{
		_assets = null!;
	}
}