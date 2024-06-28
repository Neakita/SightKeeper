using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public sealed class SimpleClassifierScreenshotsDataAccess : ClassifierScreenshotsDataAccess
{
	public override Image LoadImage(ClassifierScreenshot screenshot)
	{
		return _images[screenshot];
	}

	protected override void SaveScreenshotData(ClassifierScreenshot screenshot, Image image)
	{
		_images.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(ClassifierScreenshot screenshot)
	{
		Guard.IsTrue(_images.Remove(screenshot));
	}

	private readonly Dictionary<ClassifierScreenshot, Image> _images = new();
}