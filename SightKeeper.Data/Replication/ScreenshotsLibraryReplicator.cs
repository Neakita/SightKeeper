using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;
using PackableScreenshotsLibrary = SightKeeper.Data.Model.PackableScreenshotsLibrary;

namespace SightKeeper.Data.Replication;

internal sealed class ScreenshotsLibraryReplicator
{
	public ScreenshotsLibraryReplicator(ReplicationSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_session = session;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public IEnumerable<ScreenshotsLibrary> ReplicateScreenshotsLibraries(IEnumerable<PackableScreenshotsLibrary> packableLibraries)
	{
		return packableLibraries.Select(ReplicateScreenshotsLibrary);
	}

	private readonly ReplicationSession _session;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private ScreenshotsLibrary ReplicateScreenshotsLibrary(PackableScreenshotsLibrary packableLibrary)
	{
		ScreenshotsLibrary library = new()
		{
			Name = packableLibrary.Name,
			Description = packableLibrary.Description
		};
		foreach (var packableScreenshot in packableLibrary.Screenshots)
		{
			var screenshot = library.CreateScreenshot(packableScreenshot.CreationTimestamp, packableScreenshot.ImageSize);
			var screenshotId = packableScreenshot.Id;
			_session.Screenshots.Add(screenshotId, screenshot);
			_screenshotsDataAccess.AssociateId(screenshot, screenshotId);
		}
		return library;
	}
}