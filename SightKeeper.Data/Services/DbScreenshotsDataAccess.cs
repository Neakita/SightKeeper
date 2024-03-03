using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess, IDisposable
{
	public IObservable<Screenshot> ScreenshotAdded => _screenshotAdded.AsObservable();
	public IObservable<Screenshot> ScreenshotRemoved => _screenshotRemoved.AsObservable();
	
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Screenshot>> Load(Library library, CancellationToken cancellationToken = default)
    {
        await _dbContext.Entry(library).Collection(lib => lib.Screenshots).LoadAsync(cancellationToken);
        return library.Screenshots;
    }

    public bool IsLoaded(Library library)
    {
        return _dbContext.Entry(library).Collection(lib => lib.Screenshots).IsLoaded;
    }

    public void CreateScreenshot(Library library, byte[] content)
    {
        var screenshot = library.CreateScreenshot(content);
        _dbContext.Attach(screenshot);
        _screenshotAdded.OnNext(screenshot);
    }

    public void DeleteScreenshot(Screenshot screenshot)
    {
	    _dbContext.Attach(screenshot.Library);
	    screenshot.Library.DeleteScreenshot(screenshot);
	    _screenshotRemoved.OnNext(screenshot);
    }

    public void SaveChanges(Library library)
    {
	    _dbContext.Update(library);
	    _dbContext.SaveChanges();
    }

    public Task SaveChangesAsync(Library library, CancellationToken cancellationToken = default)
    {
	    // is Update necessary?
        _dbContext.Update(library);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly Subject<Screenshot> _screenshotAdded = new();
    private readonly Subject<Screenshot> _screenshotRemoved = new();
    private readonly AppDbContext _dbContext;

    public void Dispose()
    {
	    _screenshotAdded.Dispose();
	    _screenshotRemoved.Dispose();
    }
}