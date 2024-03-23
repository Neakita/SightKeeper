﻿using System.Collections.Immutable;
using System.Reactive.Linq;
using Serilog;

namespace SightKeeper.Application.Prediction.Handling;

public sealed class CompositeDetectionObserver : DetectionObserver
{
    public IObservable<float?> RequestedProbabilityThreshold => _observers
        .Select(observer => observer.RequestedProbabilityThreshold)
        .Aggregate((a, b) => a.CombineLatest(b).Select(t => LowestKnown(t.First, t.Second)));

    private static float? LowestKnown(float? first, float? second)
    {
        if (first == null && second == null)
            return null;
        if (first == null)
            return second;
        if (second == null)
            return first;
        return Math.Min(first.Value, second.Value);
    }
    
    public CompositeDetectionObserver(IEnumerable<DetectionObserver> observers)
    {
        _observers = observers.ToImmutableList();
        Log.ForContext<CompositeDetectionObserver>().Debug("Composite detection observer created with {Count} observers:\n{Observers}",
            _observers.Count,
            string.Join("\n", _observers.Select((observer, index) => $"#{index} {observer.GetType().Name}")));
    }
    
    public void OnNext(DetectionData data)
    {
        foreach (DetectionObserver observer in _observers)
            observer.OnNext(data);
    }

    public void OnPaused()
    {
        foreach (DetectionObserver observer in _observers)
            observer.OnPaused();
    }

    public void OnCompleted()
    {
        foreach (DetectionObserver observer in _observers)
            observer.OnCompleted();
    }

    public void OnError(Exception error)
    {
        foreach (DetectionObserver observer in _observers)
            observer.OnError(error);
    }

    private readonly ImmutableList<DetectionObserver> _observers;
}