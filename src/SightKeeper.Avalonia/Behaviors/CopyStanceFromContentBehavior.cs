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

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class CopyStanceFromContentBehavior : Behavior<Layoutable>
{
	public static readonly StyledProperty<ContentControl?> ContentControlProperty =
		AvaloniaProperty.Register<CopyStanceFromContentBehavior, ContentControl?>(nameof(ContentControl));

	public ContentControl? ContentControl
	{
		get => GetValue(ContentControlProperty);
		set => SetValue(ContentControlProperty, value);
	}

	public static readonly StyledProperty<double> FallbackMaxWidthProperty =
		AvaloniaProperty.Register<CopyStanceFromContentBehavior, double>(nameof(FallbackMaxWidth), Layoutable.MaxWidthProperty.GetDefaultValue(typeof(Layoutable)));

	public double FallbackMaxWidth
	{
		get => GetValue(FallbackMaxWidthProperty);
		set => SetValue(FallbackMaxWidthProperty, value);
	}

	public static readonly StyledProperty<double> FallbackMaxHeightProperty =
		AvaloniaProperty.Register<CopyStanceFromContentBehavior, double>(nameof(FallbackMaxHeight), Layoutable.MaxHeightProperty.GetDefaultValue(typeof(Layoutable)));

	public double FallbackMaxHeight
	{
		get => GetValue(FallbackMaxHeightProperty);
		set => SetValue(FallbackMaxHeightProperty, value);
	}

	public static readonly StyledProperty<HorizontalAlignment> FallbackHorizontalAlignmentProperty =
		AvaloniaProperty.Register<CopyStanceFromContentBehavior, HorizontalAlignment>(nameof(FallbackHorizontalAlignment), Layoutable.HorizontalAlignmentProperty.GetDefaultValue(typeof(Layoutable)));

	public HorizontalAlignment FallbackHorizontalAlignment
	{
		get => GetValue(FallbackHorizontalAlignmentProperty);
		set => SetValue(FallbackHorizontalAlignmentProperty, value);
	}

	public static readonly StyledProperty<VerticalAlignment> FallbackVerticalAlignmentProperty =
		AvaloniaProperty.Register<CopyStanceFromContentBehavior, VerticalAlignment>(nameof(FallbackVerticalAlignment), Layoutable.VerticalAlignmentProperty.GetDefaultValue(typeof(Layoutable)));

	public VerticalAlignment FallbackVerticalAlignment
	{
		get => GetValue(FallbackVerticalAlignmentProperty);
		set => SetValue(FallbackVerticalAlignmentProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		if (ContentControl == null)
			return;
		Guard.IsNull(_disposable);
		_disposable = ContentControl.ContentProperty.Changed.Subscribe(OnContentChanged);
		ContentControl.Loaded += OnContentControlLoaded;
		ContentControl.PropertyChanged += OnContentControlPropertyChanged;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_disposable);
		Guard.IsNotNull(ContentControl);
		_disposable.Dispose();
		ContentControl.Loaded -= OnContentControlLoaded;
		ContentControl.PropertyChanged -= OnContentControlPropertyChanged;
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

	private void OnContentControlPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
	{
		UpdateValues();
	}

	private void UpdateValues()
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNotNull(ContentControl);
		var presenter = (ContentPresenter?)ContentControl.GetVisualChildren().SingleOrDefault();
		if (presenter == null)
			return;
		var content = presenter.Child;
		if (content != null)
		{
			AssociatedObject.MaxWidth = content.MaxWidth;
			AssociatedObject.MaxHeight = content.MaxHeight;
			AssociatedObject.HorizontalAlignment = content.HorizontalAlignment;
			AssociatedObject.VerticalAlignment = content.VerticalAlignment;
			return;
		}
		AssociatedObject.MaxWidth = FallbackMaxWidth;
		AssociatedObject.MaxHeight = FallbackMaxHeight;
		AssociatedObject.HorizontalAlignment = FallbackHorizontalAlignment;
		AssociatedObject.VerticalAlignment = FallbackVerticalAlignment;
	}
}