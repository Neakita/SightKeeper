using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
	IObservable<Screenshot> ScreenshotAdded { get; }
	IObservable<Screenshot> ScreenshotRemoved { get; }
	
    Task<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
    bool IsLoaded(ScreenshotsLibrary library);
    void CreateScreenshot(ScreenshotsLibrary library, byte[] content);
    void DeleteScreenshot(Screenshot screenshot);
    void SaveChanges(ScreenshotsLibrary library);
    Task SaveChangesAsync(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}