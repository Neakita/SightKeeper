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

    public IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, bool byDescending)
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
            foreach (var screenshotsPartition in LoadScreenshotsPartitions(screenshotsCollectionEntry, byDescending))
                screenshotsPartitionsSubject.OnNext(screenshotsPartition);
            screenshotsPartitionsSubject.OnCompleted();
        });
        return screenshotsPartitionsSubject.AsObservable();
    }

    private IEnumerable<ImmutableList<Screenshot>> LoadScreenshotsPartitions(
        CollectionEntry<ScreenshotsLibrary, Screenshot> collectionEntry, bool byDescending)
    {
        ushort partitionIndex = 0;
        while (true)
        {
            var screenshotsPartition = LoadScreenshotsPartition(collectionEntry, PartitionSize, partitionIndex++, byDescending);
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
        ushort partitionIndex,
        bool byDescending)
    {
        var query = collectionEntry.Query();
        query = byDescending
            ? query.OrderByDescending(screenshot => EF.Property<int>(screenshot, "Id"))
            : query.OrderBy(screenshot => EF.Property<int>(screenshot, "Id"));
        query = query
            .Skip(partitionIndex * partitionSize)
            .Take(partitionSize);
        lock (_dbContext)
            return query.ToImmutableList();
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