using System.Reactive.Disposables;

namespace SightKeeper.Commons;

public static class DisposableExtensions
{
    public static T DisposeWithEx<T>(this T item, CompositeDisposable disposable)
        where T : IDisposable
    {
        disposable.Add(item);
        return item;
    }
}