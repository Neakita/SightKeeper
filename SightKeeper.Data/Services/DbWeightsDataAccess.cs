using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;
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

    public void LoadWeights(WeightsLibrary library)
    {
        _dbContext.Entry(library).Collection(lib => lib.Weights).Load();
    }

    public Task LoadWeightsAsync(WeightsLibrary library, CancellationToken cancellationToken = default)
    {
        return _dbContext.Entry(library).Collection(lib => lib.Weights).LoadAsync(cancellationToken);
    }

    public async Task<Weights> CreateWeights(
        WeightsLibrary library,
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<ItemClass> itemClasses,
        CancellationToken cancellationToken = default)
    {
        await LoadWeightsAsync(library, cancellationToken);
        var weights = library.CreateWeights(onnxData, ptData, size, metrics, itemClasses);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _weightsCreated.OnNext(weights);
        return weights;
    }

    public async Task DeleteWeights(Weights weights, CancellationToken cancellationToken)
    {
        _weightsDeleted.OnNext(weights);
        weights.Library.RemoveWeights(weights);
        await _dbContext.SaveChangesAsync(cancellationToken);
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
}