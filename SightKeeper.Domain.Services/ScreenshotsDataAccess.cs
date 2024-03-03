using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
	IObservable<Screenshot> ScreenshotAdded { get; }
	IObservable<Screenshot> ScreenshotRemoved { get; }
	
    Task<IReadOnlyCollection<Screenshot>> Load(Library library, CancellationToken cancellationToken = default);
    bool IsLoaded(Library library);
    void CreateScreenshot(Library library, byte[] content);
    void DeleteScreenshot(Screenshot screenshot);
    void SaveChanges(Library library);
    Task SaveChangesAsync(Library library, CancellationToken cancellationToken = default);
}