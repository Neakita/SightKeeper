using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

internal sealed class SimpleScreenshotsDataAccess : ScreenshotsDataAccess
{
	public override Image LoadImage(Screenshot screenshot)
	{
		return _images[screenshot];
	}

	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IEnumerable<Screenshot> screenshots, bool ordered = false)
	{
		return LoadImages(screenshots as IReadOnlyCollection<Screenshot> ?? screenshots.ToImmutableList());
	}

	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IReadOnlyCollection<Screenshot> screenshots, bool ordered = false)
	{
		return screenshots.Select(screenshot => (screenshot, _images[screenshot]));
	}

	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(DataSet dataSet)
	{
		return LoadImages(dataSet.Screenshots);
	}

	protected override void SaveScreenshotData(Screenshot screenshot, Image image)
	{
		_images.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_images.Remove(screenshot);
	}

	private readonly Dictionary<Screenshot, Image> _images = new();
}