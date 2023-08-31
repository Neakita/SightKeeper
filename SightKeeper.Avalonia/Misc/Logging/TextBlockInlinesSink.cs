using System;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Serilog.Core;
using Serilog.Events;

namespace SightKeeper.Avalonia.Misc.Logging;

public sealed class TextBlockInlinesSink : ILogEventSink
{
    public TextBlockInlinesSink(IFormatProvider? formatProvider, InlineCollection inlineCollection)
    {
        _formatProvider = formatProvider;
        _inlineCollection = inlineCollection;
    }
    
    public void Emit(LogEvent logEvent)
    {
        _inlineCollection.Add(LogEventToInline(logEvent));
    }

    private readonly IFormatProvider? _formatProvider;
    private readonly InlineCollection _inlineCollection;

    private Inline LogEventToInline(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Run run = new(message)
        {
            Foreground = LevelToBrush(logEvent.Level)
        };
        return run;
    }

    private static IBrush? LevelToBrush(LogEventLevel level) => level switch
    {
        LogEventLevel.Verbose => Brushes.Gray,
        LogEventLevel.Warning => Brushes.Yellow,
        LogEventLevel.Error => Brushes.Red,
        _ => null
    };
}