using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace SightKeeper.Avalonia;

internal static class TextBlockAssist
{
	#region AutoTrimmedTextToolTip

	public static readonly AttachedProperty<bool> AutoTrimmedTextToolTipProperty =
		AvaloniaProperty.RegisterAttached<Control, bool>("AutoTrimmedTextToolTip", typeof(VisibilityAssist));

	private static readonly ReadOnlyMemory<char> Ellipsis = "\u2026".AsMemory();

	static TextBlockAssist()
	{
		AutoTrimmedTextToolTipProperty.Changed.Subscribe(args =>
		{
			if (args.NewValue == true)
			{
				args.Sender.PropertyChanged += OnPropertyChanged;
			}
			else
			{
				args.Sender.PropertyChanged -= OnPropertyChanged;
			}
		});
	}

	public static bool GetAutoTrimmedTextToolTip(AvaloniaObject element)
	{
		return element.GetValue(AutoTrimmedTextToolTipProperty);
	}

	public static void SetAutoTrimmedTextToolTip(AvaloniaObject element, bool value)
	{
		element.SetValue(AutoTrimmedTextToolTipProperty, value);
	}
	
	private static void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
	{
		var textBlock = (TextBlock)e.Sender;
		Update(textBlock);
	}

	private static void Update(TextBlock textBlock)
	{
		ToolTip.SetTip(textBlock, IsTextBlockTrimmed(textBlock) ? textBlock.Text : null);
	}

	private static bool IsTextBlockTrimmed(TextBlock textBlock)
	{
		return textBlock.TextLayout.TextLines.SelectMany(line => line.TextRuns).Any(run => run.Text.Equals(Ellipsis));
	}

	#endregion
}