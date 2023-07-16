using SightKeeper.Application.Training.Parsing;

namespace SightKeeper.Application.Training;

public interface DarknetAdapter<TModel> where TModel : Domain.Model.Model
{
    IObservable<TrainingProgress> Progress { get; }
    int? MaxBatches { get; }
    
    Task<byte[]?> RunAsync(TModel model, byte[]? baseWeights = null, CancellationToken cancellationToken = default);
}