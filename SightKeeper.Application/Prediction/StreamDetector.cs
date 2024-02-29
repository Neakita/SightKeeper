using System.Reactive.Disposables;
using System.Reactive.Linq;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Weights;

namespace SightKeeper.Application.Prediction;

public sealed class StreamDetector(Detector detector, ScreenCapture screenCapture)
{
    private static readonly ILogger Logger = Log.ForContext<StreamDetector>();

    public Weights? Weights
    {
        get => detector.Weights;
        set
        {
            detector.Weights = value;
            screenCapture.Resolution = value?.Library.DataSet.Resolution;
        }
    }

    public float ProbabilityThreshold
    {
        get => detector.ProbabilityThreshold;
        set => detector.ProbabilityThreshold = value;
    }
    public float IoU
    {
        get => detector.IoU;
        set => detector.IoU = value;
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

    private void RunWhileNotDisposed(ICancelable cancelable, IObserver<DetectionData> observer)
    {
        while (!cancelable.IsDisposed)
        {
            using var cycleOperation = Logger.OperationAt(LogEventLevel.Debug).Begin("Detection cycle");
            var image = screenCapture.Capture();
            var items = detector.Detect(image);
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