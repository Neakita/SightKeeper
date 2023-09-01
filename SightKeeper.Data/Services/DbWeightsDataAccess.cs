﻿using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
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
        return weights;
    }

    public Task DeleteWeights(Weights weights, CancellationToken cancellationToken)
    {
        weights.Library.RemoveWeights(weights);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}