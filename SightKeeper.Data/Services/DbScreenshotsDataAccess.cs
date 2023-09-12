using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, out IObservable<int?> screenshotsCountObservable)
    {
        var entry = _dbContext.Entry(library);
        if (entry.State == EntityState.Detached)
        {
            Observable.Return(library.Screenshots);
            screenshotsCountObservable = Observable.Return<int?>(null);
        }
        Subject<IReadOnlyCollection<Screenshot>> screenshotsSubject = new();
        Subject<int?> screenshotsCountSubject = new();
        screenshotsCountObservable = screenshotsCountSubject;
        Task.Run(() =>
        {
            var collectionEntry = entry.Collection(lib => lib.Screenshots);
            int screenshotsCount;
            lock (_dbContext)
            {
                screenshotsCount = collectionEntry.Query().Count();
            }
            if (screenshotsCount == 0)
            {
                collectionEntry.Load();
                return;
            }
            screenshotsCountSubject.OnNext(screenshotsCount);
            screenshotsCountSubject.OnCompleted();
            screenshotsCountSubject.Dispose();
            Log.Debug("Screenshots to load: {Count}", screenshotsCount);
            const int partitionSize = 100;
            Log.Debug("Partition size: {Size}", partitionSize);
            var partitionsCount = (screenshotsCount + partitionSize - 1) / partitionSize;
            Log.Debug("Partitions count: {Count}", partitionsCount);
            for (var partitionIndex = 0; partitionIndex < partitionsCount; partitionIndex++)
            {
                ImmutableList<Screenshot> screenshotsPartition;
                lock (_dbContext)
                {
                    screenshotsPartition = collectionEntry.Query()
                        .OrderBy(screenshot => EF.Property<int>(screenshot, "Id"))
                        .Skip(partitionIndex * partitionSize)
                        .Take(partitionSize)
                        .ToImmutableList();
                }
                Log.Debug("Partition #{PartitionIndex} with {PartitionSize} screenshots loaded", partitionIndex, screenshotsPartition.Count);
                screenshotsSubject.OnNext(screenshotsPartition);
            }
            screenshotsSubject.OnCompleted();
            screenshotsSubject.Dispose();
        });
        return screenshotsSubject.AsObservable();
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(library);
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.SaveChanges();
            }
        }, cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}