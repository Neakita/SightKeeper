using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public sealed class SimpleDetectorScreenshotsDataAccess : DetectorScreenshotsDataAccess
{
	public override Image LoadImage(DetectorScreenshot screenshot)
	{
		return _images[screenshot];
	}

	protected override void SaveScreenshotData(DetectorScreenshot screenshot, Image image)
	{
		_images.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(DetectorScreenshot screenshot)
	{
		Guard.IsTrue(_images.Remove(screenshot));
	}

	private readonly Dictionary<DetectorScreenshot, Image> _images = new();
}