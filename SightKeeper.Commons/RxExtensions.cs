using System.Reactive.Linq;

namespace SightKeeper.Commons;

public static class RxExtensions
{
    public static IObservable<(T1, T2)> MatchByTime<T1, T2>(this IObservable<T1> firstObservable, IObservable<T2> secondsObservable, TimeSpan timeTolerance, TimeSpan throttle)
    {
        var firstTimed = firstObservable.Select(item => (item, Time: DateTime.UtcNow));
        var secondTimed = secondsObservable.Select(item => (item, Time: DateTime.UtcNow));
        var timeToleranceInMilliseconds = timeTolerance.TotalMilliseconds;
        return firstTimed.CombineLatest(secondTimed)
            .Throttle(throttle)
            .Where(t => Math.Abs((t.First.Time - t.Second.Time).TotalMilliseconds) < timeToleranceInMilliseconds)
            .Select(t => (t.First.item, t.Second.item));
    }
}