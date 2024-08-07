using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class SimpleScreenshotsDataAccess : ScreenshotsDataAccess
{
	public override Image LoadImage(Screenshot screenshot)
	{
		return _images[screenshot];
	}

	protected override void SaveScreenshotData(Screenshot screenshot, Image image)
	{
		_images.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		Guard.IsTrue(_images.Remove(screenshot));
	}

	private readonly Dictionary<Screenshot, Image> _images = new();
}