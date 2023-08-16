using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class DetectorTrainer : ModelTrainer<DetectorDataSet>
{
    public DetectorDataSet? Model { get; set; }
    public bool FromScratch { get; set; }
    public int? MaxBatches => _darknetAdapter.MaxBatches;
    public IObservable<TrainingProgress> Progress => _darknetAdapter.Progress;

    public DetectorTrainer(DarknetAdapter<DetectorDataSet> darknetAdapter, WeightsDataAccess weightsDataAccess)
    {
        _darknetAdapter = darknetAdapter;
        _weightsDataAccess = weightsDataAccess;
        Progress.Subscribe(progress => _lastProgress = progress);
    }
    
    public async Task<InternalTrainedModelWeights?> TrainAsync(ModelConfig config, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(Model);
        var assets = Model.Assets.ToList();
        _weightsDataAccess.LoadWeights(Model.WeightsLibrary);
        var baseWeights = FromScratch ? null : Model.WeightsLibrary.Weights.MaxBy(weights => weights.TrainedDate);
        var weightsData = await _darknetAdapter.RunAsync(Model, config, baseWeights?.Data, cancellationToken);
        if (weightsData == null)
            return null;
        Guard.IsNotNull(_lastProgress);
        var weights = _weightsDataAccess.CreateWeights(
            Model.WeightsLibrary,
            weightsData,
            DateTime.Now,
            config,
            (int)_lastProgress.Value.Batch,
            _lastProgress.Value.AverageLoss,
            null,
            assets);
        _lastProgress = null;
        return weights;
    }

    private readonly DarknetAdapter<DetectorDataSet> _darknetAdapter;
    private readonly WeightsDataAccess _weightsDataAccess;
    private TrainingProgress? _lastProgress;
}
