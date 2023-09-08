using System.Reactive.Linq;
using System.Reactive.Subjects;
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

    public Task LoadWeights(WeightsLibrary library, CancellationToken cancellationToken = default) =>
        _dbContext.Entry(library).Collection(lib => lib.Weights).LoadAsync(cancellationToken: cancellationToken);

    public async Task<Weights> CreateWeights(
        WeightsLibrary library,
        byte[] data,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<Asset> assets,
        CancellationToken cancellationToken = default)
    {
        await LoadWeights(library, cancellationToken);
        var weights = library.CreateWeights(data, size, epoch, boundingLoss, classificationLoss, deformationLoss, assets);
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

    private readonly AppDbContext _dbContext;
    private readonly Subject<Weights> _weightsCreated = new();
    private readonly Subject<Weights> _weightsDeleted = new();
}