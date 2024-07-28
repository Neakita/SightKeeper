using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class TrimmedTextToolTipBehavior : Behavior<TextBlock>
{
	private static readonly ReadOnlyMemory<char> Ellipsis = "\u2026".AsMemory();

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PropertyChanged += OnAssociatedObjectPropertyChanged;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PropertyChanged -= OnAssociatedObjectPropertyChanged;
	}

	private void OnAssociatedObjectPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs args)
	{
		UpdateToolTip();
	}

	private void UpdateToolTip()
	{
		Guard.IsNotNull(AssociatedObject);
		ToolTip.SetTip(AssociatedObject, IsTextBlockTrimmed(AssociatedObject) ? AssociatedObject.Text : null);
	}

	private static bool IsTextBlockTrimmed(TextBlock textBlock)
	{
		return textBlock.TextLayout.TextLines.SelectMany(line => line.TextRuns).Any(run => run.Text.Equals(Ellipsis));
	}
}