using Serilog;
using Serilog.Core;

namespace SightKeeper.Avalonia.Compositions;

public static class LoggerBootstrapper
{
	public static Logger Logger { get; } =
		new LoggerConfiguration()
			.WriteTo.Debug()
			.CreateLogger();
}