using Serilog;

namespace SightKeeper.Application.Extensions;

public static class Extensions
{
    public static ILogger WithGlobal(this ILogger? logger) => logger == null
        ? Log.Logger
        : new LoggerConfiguration().WriteTo.Logger(logger).WriteTo.Logger(Log.Logger).CreateLogger();
}