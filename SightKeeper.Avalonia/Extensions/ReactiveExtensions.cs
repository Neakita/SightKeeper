using System;
using System.Reactive.Linq;

namespace SightKeeper.Avalonia.Extensions;

internal static class ReactiveExtensions
{
	public static IObservable<T> ToObservable<T>(this EventHandler<T>? handler)
	{
		return Observable.FromEventPattern<T>(
				h => handler += h,
				h => handler -= h)
			.Select(x => x.EventArgs);
	}
}