using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model.Model;

namespace SightKeeper.Application.Training;

public interface DarknetAdapter<TModel> where TModel : Model
{
    IObservable<TrainingProgress> Progress { get; }
    int? MaxBatches { get; }
    
    Task<byte[]?> RunAsync(TModel model, byte[]? baseWeights = null, CancellationToken cancellationToken = default);
}