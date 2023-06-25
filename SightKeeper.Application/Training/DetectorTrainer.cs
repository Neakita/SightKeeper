﻿using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model.Common;
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
    
    public async Task<ModelWeights?> TrainAsync(CancellationToken cancellationToken = default)
    {
        if (Model == null) ThrowHelper.ThrowArgumentNullException(nameof(Model));
        var assets = Model.Assets.ToList();
        var baseWeights = FromScratch ? null : Model.Weights.MaxBy(weights => weights.Date);
        var weightsData = await _darknetAdapter.RunAsync(Model, baseWeights?.Data, cancellationToken);
        if (weightsData == null) return null;
        return new ModelWeights(0, weightsData, assets);
    }
}
