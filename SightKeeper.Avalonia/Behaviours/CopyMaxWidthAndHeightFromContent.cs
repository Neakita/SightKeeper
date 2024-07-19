using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviours;

internal sealed class CopyMaxWidthAndHeightFromContent : Behavior<Layoutable>
{
	public static readonly StyledProperty<ContentControl?> ContentControlProperty =
		AvaloniaProperty.Register<CopyMaxWidthAndHeightFromContent, ContentControl?>(nameof(ContentControl));

	public static readonly StyledProperty<double> FallbackMaxWidthProperty =
		AvaloniaProperty.Register<CopyMaxWidthAndHeightFromContent, double>(nameof(FallbackMaxWidth), Layoutable.MaxWidthProperty.GetDefaultValue(typeof(Layoutable)));

	public static readonly StyledProperty<double> FallbackMaxHeightProperty =
		AvaloniaProperty.Register<CopyMaxWidthAndHeightFromContent, double>(nameof(FallbackMaxHeight), Layoutable.MaxHeightProperty.GetDefaultValue(typeof(Layoutable)));

	public ContentControl? ContentControl
	{
		get => GetValue(ContentControlProperty);
		set => SetValue(ContentControlProperty, value);
	}

	public double FallbackMaxWidth
	{
		get => GetValue(FallbackMaxWidthProperty);
		set => SetValue(FallbackMaxWidthProperty, value);
	}

	public double FallbackMaxHeight
	{
		get => GetValue(FallbackMaxHeightProperty);
		set => SetValue(FallbackMaxHeightProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		if (ContentControl == null)
			return;
		Guard.IsNull(_disposable);
		_disposable = ContentControl.ContentProperty.Changed.Subscribe(OnContentChanged);
		ContentControl.Loaded += OnContentControlLoaded;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_disposable);
		Guard.IsNotNull(ContentControl);
		_disposable.Dispose();
		ContentControl.Loaded -= OnContentControlLoaded;
	}

	private IDisposable? _disposable;

	private void OnContentChanged(AvaloniaPropertyChangedEventArgs<object?> args)
	{
		if (args.Sender != ContentControl)
			return;
		UpdateValues();
	}

	private void OnContentControlLoaded(object? sender, RoutedEventArgs e)
	{
		UpdateValues();
	}

	private void UpdateValues()
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(ContentControl);
		var presenter = (ContentPresenter)ContentControl.GetVisualChildren().Single();
		var content = presenter.Child;
		if (content != null)
		{
			AssociatedObject.MaxWidth = content.MaxWidth;
			AssociatedObject.MaxHeight = content.MaxHeight;
			return;
		}
		AssociatedObject.MaxWidth = FallbackMaxWidth;
		AssociatedObject.MaxHeight = FallbackMaxHeight;
	}
}