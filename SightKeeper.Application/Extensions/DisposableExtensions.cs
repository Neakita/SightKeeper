using System.Reactive.Disposables;

namespace SightKeeper.Application.Extensions;

internal static class DisposableExtensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}
}