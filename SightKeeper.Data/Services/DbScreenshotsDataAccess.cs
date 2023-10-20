using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SerilogTimings;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
    private const int PartitionSize = 100;
    
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IReadOnlyCollection<Screenshot>> LoadAll(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.Entry(library).Collection(lib => lib.Screenshots).Load();
            }

            return library.Screenshots;
        }, cancellationToken);
    }

    public IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, bool byDescending)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (library.Screenshots != null)
        {
            Log.Debug("Screenshots already loaded, returning {Count} screenshots", library.Screenshots.Count);
            return Observable.Return(library.Screenshots);
        }

        Subject<IReadOnlyCollection<Screenshot>> screenshotsPartitionsSubject = new();
        Task.Run(() =>
        {
            Log.Debug("Screenshots aren't loaded yet, waiting for database context locking...");
            IQueryable<Screenshot> screenshotsQuery;
            lock (_dbContext)
                screenshotsQuery = _dbContext.Entry(library).Collection(lib => lib.Screenshots).Query();
            Log.Debug("Screenshots collection entry retrieved");
            LoadScreenshotsPartitions(screenshotsQuery, byDescending).ToObservable().Subscribe(screenshotsPartitionsSubject);
        });
        return screenshotsPartitionsSubject.AsObservable();
    }

    public Task<bool> IsLoaded(ScreenshotsLibrary library)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
                return _dbContext.Entry(library).Collection(lib => lib.Screenshots).IsLoaded;
        }); 
    }

    public Task CreateScreenshot(ScreenshotsLibrary library, byte[] content, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                var screenshot = library.CreateScreenshot(content);
                _dbContext.Attach(screenshot);
            }
        }, cancellationToken);
    }

    private IEnumerable<ImmutableList<Screenshot>> LoadScreenshotsPartitions(
        IQueryable<Screenshot> screenshotsQuery, bool byDescending)
    {
        ushort partitionIndex = 0;
        while (true)
        {
            var screenshotsPartition = LoadScreenshotsPartition(screenshotsQuery, PartitionSize, partitionIndex++, byDescending);
            if (screenshotsPartition.Count == 0)
                break;
            yield return screenshotsPartition;
            if (screenshotsPartition.Count < PartitionSize)
                break;
        }
    }

    private ImmutableList<Screenshot> LoadScreenshotsPartition(
        IQueryable<Screenshot> query,
        ushort partitionSize,
        ushort partitionIndex,
        bool byDescending)
    {
        using var operation = Operation.Begin("Loading screenshots partition #{PartitionIndex}", partitionIndex);
        query = byDescending
            ? query.OrderByDescending(screenshot => EF.Property<int>(screenshot, "Id"))
            : query.OrderBy(screenshot => EF.Property<int>(screenshot, "Id"));
        query = query
            .Skip(partitionIndex * partitionSize)
            .Take(partitionSize);
        Log.Debug("Query for screenshots partition #{PartitionIndex} prepared, waiting for database context locking...", partitionIndex);
        ImmutableList<Screenshot> screenshotsPartition;
        lock (_dbContext)
            screenshotsPartition = query.ToImmutableList();
        operation.Complete(nameof(screenshotsPartition.Count), screenshotsPartition.Count);
        return screenshotsPartition;
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.Update(library);
                _dbContext.SaveChanges();
            }
        }, cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}