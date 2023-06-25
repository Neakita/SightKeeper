using System.Reactive.Linq;
using Serilog;

namespace SightKeeper.Application;

public static class Extensions
{
    public static ILogger WithGlobal(this ILogger? logger) => logger == null
        ? Log.Logger
        : new LoggerConfiguration().WriteTo.Logger(logger).WriteTo.Logger(Log.Logger).CreateLogger();

    public static IObservable<T> WhereNotNull<T>(this IObservable<T?> observable) where T : struct =>
        observable.Where(item => item != null).Select(item => item!.Value);
}