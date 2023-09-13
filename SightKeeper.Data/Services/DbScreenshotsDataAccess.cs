using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (library.Screenshots != null)
            return Observable.Return(library.Screenshots);
        Subject<IReadOnlyCollection<Screenshot>> screenshotsPartitionsSubject = new();
        Task.Run(() =>
        {
            CollectionEntry<ScreenshotsLibrary, Screenshot> screenshotsCollectionEntry;
            lock (_dbContext)
            {
                var libraryEntry = _dbContext.Entry(library);
                screenshotsCollectionEntry = libraryEntry.Collection(lib => lib.Screenshots);
            }
            foreach (var screenshotsPartition in LoadScreenshotsPartitions(screenshotsCollectionEntry))
                screenshotsPartitionsSubject.OnNext(screenshotsPartition);
            screenshotsPartitionsSubject.OnCompleted();
        });
        return screenshotsPartitionsSubject.AsObservable();
    }

    private IEnumerable<ImmutableList<Screenshot>> LoadScreenshotsPartitions(
        CollectionEntry<ScreenshotsLibrary, Screenshot> collectionEntry)
    {
        ushort partitionIndex = 0;
        while (true)
        {
            var screenshotsPartition = LoadScreenshotsPartition(collectionEntry, PartitionSize, partitionIndex++);
            if (screenshotsPartition.Count == 0)
                break;
            yield return screenshotsPartition;
            if (screenshotsPartition.Count < PartitionSize)
                break;
        }
    }

    private ImmutableList<Screenshot> LoadScreenshotsPartition(
        CollectionEntry<ScreenshotsLibrary, Screenshot> collectionEntry,
        ushort partitionSize,
        ushort partitionIndex)
    {
        lock (_dbContext)
        {
            return collectionEntry.Query()
                .OrderBy(screenshot => EF.Property<int>(screenshot, "Id"))
                .Skip(partitionIndex * partitionSize)
                .Take(partitionSize)
                .ToImmutableList();
        }
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