using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using SerilogTimings;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
    public IObservable<Weights> WeightsCreated => _weightsCreated.AsObservable();
    public IObservable<Weights> WeightsDeleted => _weightsDeleted.AsObservable();
    
    public DbWeightsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task LoadAllWeights(WeightsLibrary library, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.Entry(library).Collection(lib => lib.Weights).Load();
            }
        }, cancellationToken);
    }

    public IObservable<Weights> LoadWeights(WeightsLibrary library, CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (library.Weights != null)
            return library.Weights.ToObservable();
        Subject<Weights> weightsSubject = new();
        Task.Run(() =>
        {
            IQueryable<Weights> weightsQuery;
            lock (_dbContext)
                weightsQuery = _dbContext.Entry(library).Collection(weights => weights.Weights).Query();
            return LoadWeights(weightsQuery).ToObservable().Subscribe(weightsSubject);
        }, cancellationToken);
        return weightsSubject;
    }

    public async Task<Weights> CreateWeights(
        WeightsLibrary library,
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<Asset> assets,
        CancellationToken cancellationToken = default)
    {
        await LoadAllWeights(library, cancellationToken);
        var weights = library.CreateWeights(onnxData, ptData, size, epoch, boundingLoss, classificationLoss, deformationLoss, assets);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _weightsCreated.OnNext(weights);
        return weights;
    }

    public async Task DeleteWeights(Weights weights, CancellationToken cancellationToken)
    {
        weights.Library.RemoveWeights(weights);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _weightsDeleted.OnNext(weights);
    }

    public async Task<WeightsData> LoadWeightsData(Weights weights, WeightsFormat format, CancellationToken cancellationToken = default)
    {
        var weightsEntry = _dbContext.Entry(weights);
        return format switch
        {
            WeightsFormat.PT => await LoadWeightsData(weightsEntry, w => w.PTData, cancellationToken),
            WeightsFormat.ONNX => await LoadWeightsData(weightsEntry, w => w.ONNXData, cancellationToken),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<WeightsData>(nameof(format), format, null)
        };
    }
    
    private static Task<TData> LoadWeightsData<TData>(EntityEntry<Weights> weightsEntry, Expression<Func<Weights, TData?>> propertyExpression, CancellationToken cancellationToken) where TData : WeightsData
    {
        var query = weightsEntry.Reference(propertyExpression).Query();
        return query.AsNoTracking().SingleAsync(cancellationToken: cancellationToken);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Weights> _weightsCreated = new();
    private readonly Subject<Weights> _weightsDeleted = new();

    private IEnumerable<Weights> LoadWeights(IQueryable<Weights> weightsQuery)
    {
        var index = 0;
        while (true)
        {
            var weights = LoadWeights(weightsQuery, index++);
            if (weights == null)
                break;
            yield return weights;
        }
    }

    private Weights? LoadWeights(IQueryable<Weights> weightsQuery, int index)
    {
        using var operation = Operation.Begin("Loading weights #{Index}", index);
        Log.Debug("Waiting for database context locking to load weights #{Index}...", index);
        lock (_dbContext)
            return weightsQuery
                .OrderBy(weights => EF.Property<int>(weights, "Id"))
                .Skip(index)
                .FirstOrDefault();
    }
}