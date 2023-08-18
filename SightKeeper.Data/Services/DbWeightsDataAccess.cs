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

    public Weights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelSize size,
        int epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets)
    {
        var weights = library.CreateWeights(data, trainedDate, size, epoch, boundingLoss, classificationLoss, assets);
        _dbContext.SaveChanges();
        return weights;
    }
    
    private readonly AppDbContext _dbContext;
}