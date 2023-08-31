using System;
using Avalonia.Controls.Documents;
using Serilog;
using Serilog.Configuration;

namespace SightKeeper.Avalonia.Misc.Logging;

public static class LoggingExtensions
{
    public static LoggerConfiguration TextBlockInlines(
        this LoggerSinkConfiguration  loggerConfiguration,
        InlineCollection inlineCollection,
        IFormatProvider? formatProvider = null) =>
        loggerConfiguration.Sink(new TextBlockInlinesSink(formatProvider, inlineCollection));
}