using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public interface DarknetAdapter<TModel> where TModel : Domain.Model.Model
{
    IObservable<TrainingProgress> Progress { get; }
    int? MaxBatches { get; }
    
    Task<byte[]?> RunAsync(TModel model, ModelConfig config, byte[]? baseWeights = null, CancellationToken cancellationToken = default);
}