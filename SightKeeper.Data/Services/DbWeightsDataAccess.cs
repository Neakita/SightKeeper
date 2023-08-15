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

    public void LoadWeights(ModelWeightsLibrary library) => _dbContext.Entry(library).Collection(lib => lib.Weights).Load();

    public InternalTrainedModelWeights CreateWeights(
        ModelWeightsLibrary library,
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        int batch,
        float averageLoss,
        float? accuracy,
        IEnumerable<Asset> assets)
    {
        var weights = library.CreateWeights(data, trainedDate, config, batch, averageLoss, accuracy, assets);
        _dbContext.SaveChanges();
        return weights;
    }

    public PreTrainedModelWeights CreateWeights(
        ModelWeightsLibrary library,
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