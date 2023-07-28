using Autofac;
using SightKeeper.Application.Training;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Training;

public sealed class TrainingService
{
    public TrainingService(ILifetimeScope scope)
    {
        _scope = scope;
    }
    
    public async Task TrainAsync(Model model, CancellationToken cancellationToken = default)
    {
        if (model is DetectorModel detectorModel)
        {
            var trainer = _scope.Resolve<ModelTrainer<DetectorModel>>();
            trainer.Model = detectorModel;
            var weights = await trainer.TrainAsync(cancellationToken);
            if (weights != null)
            {
                var dbContext = _scope.Resolve<AppDbContext>();
                dbContext.Attach(detectorModel);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        throw new NotSupportedException($"Model of type {model.GetType()} is not supported");
    }
    
    private readonly ILifetimeScope _scope;
}