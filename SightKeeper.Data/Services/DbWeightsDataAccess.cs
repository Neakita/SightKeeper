using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
    public DbWeightsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void LoadWeights(WeightsLibrary library) => _dbContext.Entry(library).Collection(lib => lib.Weights).Load();

    public InternalTrainedWeights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        int epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets)
    {
        var weights = library.CreateWeights(data, trainedDate, config, epoch, boundingLoss, classificationLoss, assets);
        _dbContext.SaveChanges();
        return weights;
    }

    public PreTrainedWeights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        DateTime addedDate)
    {
        var weights = library.CreateWeights(data, trainedDate, config, addedDate);
        _dbContext.SaveChanges();
        return weights;
    }
    
    private readonly AppDbContext _dbContext;
}