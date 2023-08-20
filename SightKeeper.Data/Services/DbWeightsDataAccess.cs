using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
    public DbWeightsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void LoadWeights(WeightsLibrary library) =>
        _dbContext.Entry(library).Collection(lib => lib.Weights).Load();

    public Weights CreateWeights(
        WeightsLibrary library,
        byte[] data,
        ModelSize size,
        uint epoch,
        float loss)
    {
        var weights = library.CreateWeights(data, size, epoch, loss);
        _dbContext.SaveChanges();
        return weights;
    }
    
    private readonly AppDbContext _dbContext;
}