using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Prediction;

public sealed class StreamDetector
{
    public IObservable<DetectionData> Detection => _detection;
    public bool CheckImagesEquality { get; set; } = true;

    public bool IsEnabled
    {
        get => _isEnabledDisposable != null;
        set
        {
            if (value == IsEnabled)
                return;
            if (value)
            {
                Guard.IsNull(_isEnabledDisposable);
                var previousImage = Array.Empty<byte>();
                _isEnabledDisposable = Observable.Create<DetectionData>(observer =>
                {
                    var work = true;
                    Task.Run(async () =>
                    {
                        while (work)
                        {
                            using var operation = _logger.OperationAt(LogEventLevel.Debug).Begin("Detection cycle");
                            using var screenCaptureOperation = _logger.OperationAt(LogEventLevel.Debug).Begin("Screen capture");
                            var image = await _screenCapture.CaptureAsync();
                            screenCaptureOperation.Complete(nameof(image), image.Length);
                            if (CheckImagesEquality)
                            {
                                using var imagesComparisonOperation = _logger.OperationAt(LogEventLevel.Debug).Begin("Comparing images");
                                var imagesAreEqual = image.SequenceEqual(previousImage);
                                imagesComparisonOperation.Complete(nameof(imagesAreEqual), imagesAreEqual);
                                if (imagesAreEqual)
                                {
                                    Log.Debug("Images are equal, skipping...");
                                    continue;
                                }
                                previousImage = image;
                            }
                            using var detectOperation = _logger.OperationAt(LogEventLevel.Debug).Begin("Detection");
                            var result = await _detector.Detect(image, CancellationToken.None);
                            detectOperation.Complete(nameof(result), result);
                            if (IsEnabled)
                            {
                                using var handlingOperation = _logger.OperationAt(LogEventLevel.Debug).Begin("Detection handling");
                                observer.OnNext(new DetectionData(image, result.ToImmutableList()));
                                handlingOperation.Complete();
                            }
                            operation.Complete();
                        }
                    });
                    return Disposable.Create(() => work = false);
                }).Subscribe(_detection);
            }
            else
            {
                Guard.IsNotNull(_isEnabledDisposable);
                _isEnabledDisposable.Dispose();
                _isEnabledDisposable = null;
            }
        }
    }

    public Weights? Weights
    {
        get => _detector.Weights;
        set
        {
            _detector.Weights = value;
            _screenCapture.Resolution = value?.Library.DataSet.Resolution;
        }
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
    
    private readonly Detector _detector;
    private readonly ScreenCapture _screenCapture;
    private IDisposable? _isEnabledDisposable;
    private readonly Subject<DetectionData> _detection = new();
    private readonly ILogger _logger = Log.ForContext<StreamDetector>();
}