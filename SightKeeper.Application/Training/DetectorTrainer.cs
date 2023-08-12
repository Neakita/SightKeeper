using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training;

public sealed class DetectorTrainer : ModelTrainer<DetectorModel>
{
    private readonly DarknetAdapter<DetectorModel> _darknetAdapter;
    public DetectorModel? Model { get; set; }
    public bool FromScratch { get; set; }
    public int? MaxBatches => _darknetAdapter.MaxBatches;
    public IObservable<TrainingProgress> Progress => _darknetAdapter.Progress;

    public DetectorTrainer(DarknetAdapter<DetectorModel> darknetAdapter)
    {
        _darknetAdapter = darknetAdapter;
    }
    
    public async Task<InternalTrainedModelWeights?> TrainAsync(ModelConfig config, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(Model);
        var assets = Model.Assets.ToList();
        var baseWeights = FromScratch ? null : Model.WeightsLibrary.Weights.MaxBy(weights => weights.TrainedDate);
        var weightsData = await _darknetAdapter.RunAsync(Model, config, baseWeights?.Data, cancellationToken);
        if (weightsData == null) return null;
        var lastProgress = await Progress.LastAsync();
        Guard.IsNotNull(lastProgress.Batch);
        Guard.IsNotNull(lastProgress.Accuracy);
        return Model.WeightsLibrary.CreateWeights(weightsData, DateTime.Now, config, Model.WeightsLibrary, (int)lastProgress.Batch.Value, lastProgress.Accuracy.Value, assets);
    }
}
