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

    public IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library)
    {
        var entry = _dbContext.Entry(library);
        if (entry.State == EntityState.Detached)
            Observable.Return(library.Screenshots);
        var subject = new Subject<IReadOnlyCollection<Screenshot>>();
        Task.Run(() =>
        {
            var collectionEntry = entry.Collection(lib => lib.Screenshots);
            int screenshotsCount;
            lock (_dbContext)
            {
                screenshotsCount = collectionEntry.Query().Count();
            }
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
                subject.OnNext(screenshotsPartition);
            }
            subject.OnCompleted();
            subject.Dispose();
        });
        return subject.AsObservable();
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(library);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}