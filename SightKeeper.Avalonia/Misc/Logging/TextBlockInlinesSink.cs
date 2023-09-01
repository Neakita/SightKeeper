using System;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Threading;
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
        Dispatcher.UIThread.Invoke(() =>
        {
            _inlineCollection.Add(LogEventToInline(logEvent));
            if (_inlineCollection.Count > 200)
                _inlineCollection.RemoveAt(0);
        });
    }

    private readonly IFormatProvider? _formatProvider;
    private readonly InlineCollection _inlineCollection;

    private Inline LogEventToInline(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Run run = new(message + '\n');
        var brush = LevelToBrush(logEvent.Level);
        if (brush != null)
            run.Foreground = brush;
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