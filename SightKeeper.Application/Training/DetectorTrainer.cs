using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training;

public sealed class DetectorTrainer : ModelTrainer<DetectorModel>
{
    public DetectorModel? Model { get; set; }
    public bool FromScratch { get; set; }
    public uint? MaxBatches { get; }
    public IObservable<TrainingProgress> Progress => _progressSubject.AsObservable();

    public DetectorTrainer(DarknetHelper darknetHelper, DarknetOutputParser<DetectorModel> outputParser)
    {
        _darknetHelper = darknetHelper;
        _outputParser = outputParser;
    }
    
    public async Task<ModelWeights?> TrainAsync(CancellationToken cancellationToken = default)
    {
        if (Model == null) ThrowHelper.ThrowArgumentNullException(nameof(Model));
        var trainer = _darknetHelper.StartNewTrainer(Model);
        if (trainer.Process == null) ThrowHelper.ThrowArgumentNullException(nameof(trainer.Process));
        var outputDisposable = trainer.OutputReceived
            .Select(output => _outputParser.TryParse(output, out var progress) ? progress : null)
            .Where(progress => progress != null)
            .Select(progress => progress!.Value)
            .Subscribe(progress => _progressSubject.OnNext(progress));
        string? weightsFilePath;
        try
        {
            await trainer.Process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException exception)
        {
            Log.Verbose(exception, "Training was cancelled");
        }
        finally
        {
            weightsFilePath = DarknetHelper.GetLastWeightsFilePath();
            outputDisposable.Dispose();
            trainer.Dispose();
            DarknetHelper.ClearDataDirectory();
        }
        if (weightsFilePath == null) return null;
        return new ModelWeights(0, await File.ReadAllBytesAsync(weightsFilePath, CancellationToken.None), Model.DetectorScreenshots);
    }
    
    private readonly DarknetHelper _darknetHelper;
    private readonly DarknetOutputParser<DetectorModel> _outputParser;
    private readonly Subject<TrainingProgress> _progressSubject = new();
}
