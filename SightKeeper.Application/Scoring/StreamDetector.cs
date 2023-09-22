using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Events;
using SerilogTimings;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Scoring;

public sealed class StreamDetector
{
    public IObservable<(byte[] imageData, ImmutableList<DetectionItem> items)> ObservableDetection => _detection;
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
                _isEnabledDisposable = Observable.Create<(byte[], ImmutableList<DetectionItem>)>(observer =>
                {
                    var work = true;
                    Task.Run(async () =>
                    {
                        while (work)
                        {
                            using var operation = Operation.At(LogEventLevel.Verbose).Begin("Detection cycle");
                            var image = await _screenCapture.CaptureAsync();
                            if (CheckImagesEquality)
                            {
                                using var imagesComparisonOperation = Operation.Begin("Comparing images");
                                var imagesAreEqual = image.SequenceEqual(previousImage);
                                imagesComparisonOperation.Complete(nameof(imagesAreEqual), imagesAreEqual);
                                if (imagesAreEqual)
                                {
                                    Log.Debug("Images are equal, skipping...");
                                    continue;
                                }

                                previousImage = image;
                            }

                            var result = await _detector.Detect(image, CancellationToken.None);
                            observer.OnNext((image, result.ToImmutableList()));
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
    private readonly Subject<(byte[], ImmutableList<DetectionItem>)> _detection = new();
}