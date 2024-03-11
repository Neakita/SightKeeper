using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.Prediction;

public sealed class StreamDetector
{
    private static readonly ILogger Logger = Log.ForContext<StreamDetector>();

    public Weights? Weights
    {
        get => _detector.Weights;
        set => _detector.Weights = value;
    }

    public float ProbabilityThreshold
    {
        get => _detector.ProbabilityThreshold;
        set => _detector.ProbabilityThreshold = value;
    }
    public float IoU
    {
        get => _detector.IoU;
        set => _detector.IoU = value;
    }

    public StreamDetector(Detector detector, ScreenCapture screenCapture)
    {
	    _detector = detector;
	    _screenCapture = screenCapture;
    }

    public IObservable<DetectionData> RunObservable()
    {
        return Observable.Create<DetectionData>(observer =>
        {
            BooleanDisposable disposable = new();
            Task.Factory.StartNew(() => RunWhileNotDisposed(disposable, observer), TaskCreationOptions.LongRunning);
            return disposable;
        });
    }

    private readonly Detector _detector;
    private readonly ScreenCapture _screenCapture;

    private void RunWhileNotDisposed(ICancelable cancelable, IObserver<DetectionData> observer)
    {
	    Guard.IsNotNull(Weights);
	    var dataSet = Weights.Library.DataSet;
	    var resolution = dataSet.Resolution;
	    var game = dataSet.Game;
        while (!cancelable.IsDisposed)
        {
            using var cycleOperation = Logger.OperationAt(LogEventLevel.Debug).Begin("Detection cycle");
            var image = _screenCapture.Capture(resolution, game);
            var items = _detector.Detect(image);
            DetectionData detectionData = new(image, items);
            HandleDetection(observer, detectionData);
            cycleOperation.Complete();
        }
    }

    private static void HandleDetection(IObserver<DetectionData> observer, DetectionData detectionData)
    {
        using var handlingOperation = Logger.OperationAt(LogEventLevel.Debug).Begin("Detection handling");
        observer.OnNext(detectionData);
        handlingOperation.Complete();
    }
}