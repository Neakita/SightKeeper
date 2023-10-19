using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Services.Prediction.Handling;

public sealed class DetectionScreenshotingParameters
{
    public bool MakeScreenshots { get; set; }

    public IObservable<float> ObservableMinimumProbability => _minimumProbabilitySubject;

    public float MinimumProbability
    {
        get => _minimumProbabilitySubject.Value;
        set
        {
            Guard.IsBetween(value, 0, 1);
            Guard.IsLessThanOrEqualTo(value, MaximumProbability);
            _minimumProbabilitySubject.OnNext(value);
        }
    }

    public float MaximumProbability
    {
        get => _maximumProbability;
        set
        {
            Guard.IsBetween(value, 0, 1);
            Guard.IsGreaterThanOrEqualTo(value, MinimumProbability);
            _maximumProbability = value;
        }
    }

    public byte MaximumFPS
    {
        get => _maximumFPS;
        set
        {
            Guard.IsGreaterThan(value, (byte)0);
            _maximumFPS = value;
            ScreenshotingDelay = TimeSpan.FromSeconds(1f / _maximumFPS);
        }
    }

    public TimeSpan ScreenshotingDelay = TimeSpan.FromSeconds(1f / DefaultMaximumFPS);

    private const byte DefaultMaximumFPS = 1;
    private float _maximumProbability = 0.5f;
    private byte _maximumFPS = DefaultMaximumFPS;
    private readonly BehaviorSubject<float> _minimumProbabilitySubject = new(0.2f);
}