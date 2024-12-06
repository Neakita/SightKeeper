using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using PackableScreenshotsLibrary = SightKeeper.Data.Binary.Model.PackableScreenshotsLibrary;

namespace SightKeeper.Data.Binary.Replication.DataSets;

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
		ScreenshotsLibrary library = new(packableLibrary.Name);
		foreach (var packableScreenshot in packableLibrary.Screenshots)
		{
			var screenshot = library.CreateScreenshot(packableScreenshot.CreationDate, packableScreenshot.ImageSize);
			var screenshotId = packableScreenshot.Id;
			_session.Screenshots.Add(screenshotId, screenshot);
			_screenshotsDataAccess.AssociateId(screenshot, screenshotId);
		}
		return library;
	}
}