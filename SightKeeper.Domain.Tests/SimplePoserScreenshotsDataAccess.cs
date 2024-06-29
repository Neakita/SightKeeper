using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public sealed class SimplePoserScreenshotsDataAccess : PoserScreenshotsDataAccess
{
	public override Image LoadImage(PoserScreenshot screenshot)
	{
		return _images[screenshot];
	}

	protected override void SaveScreenshotData(PoserScreenshot screenshot, Image image)
	{
		_images.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(PoserScreenshot screenshot)
	{
		Guard.IsTrue(_images.Remove(screenshot));
	}

	private readonly Dictionary<PoserScreenshot, Image> _images = new();
}