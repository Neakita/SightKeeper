using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class RecyclableScreenshotImageBindingBehavior : Behavior<Image>
{
	public static readonly StyledProperty<Screenshot?> ScreenshotProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, Screenshot?>(nameof(Screenshot));

	public static readonly StyledProperty<ScreenshotImageLoader?> ImageLoaderProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, ScreenshotImageLoader?>(nameof(ImageLoader));

	public static readonly StyledProperty<Size> TargetSizeProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, Size>(nameof(TargetSize));

	public static readonly StyledProperty<int> SizeStepProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, int>(nameof(SizeStep), 20);

	public static readonly StyledProperty<int> MinimumSizeProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, int>(nameof(MinimumSize), 20);

	public Screenshot? Screenshot
	{
		get => GetValue(ScreenshotProperty);
		set => SetValue(ScreenshotProperty, value);
	}

	public ScreenshotImageLoader? ImageLoader
	{
		get => GetValue(ImageLoaderProperty);
		set => SetValue(ImageLoaderProperty, value);
	}

	public Size TargetSize
	{
		get => GetValue(TargetSizeProperty);
		set => SetValue(TargetSizeProperty, value);
	}

	public int SizeStep
	{
		get => GetValue(SizeStepProperty);
		set => SetValue(SizeStepProperty, value);
	}

	public int MinimumSize
	{
		get => GetValue(MinimumSizeProperty);
		set => SetValue(MinimumSizeProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == TargetSizeProperty)
		{
			var (oldSize, newSize) = change.GetOldAndNewValue<Size>();
			if (Math.Abs(Math.Max(oldSize.Width, oldSize.Height) - Math.Max(newSize.Width, newSize.Height)) < 1)
				return;
		}

		if (change.Property != ImageLoaderProperty)
			UpdateSource();
		else
		{
			var oldImageLoader = change.GetOldValue<ScreenshotImageLoader?>();
			UpdateSource(oldImageLoader);
		}
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Source = _bitmap;
		AssociatedObject.Loaded += OnAssociatedObjectLoaded;
	}

	private void OnAssociatedObjectLoaded(object? sender, RoutedEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
		UpdateSource();
	}

	protected override void OnDetachedFromVisualTree()
	{
		if (_bitmap == null)
			return;
		Guard.IsNotNull(ImageLoader);
		ImageLoader.ReturnBitmapToPool(_bitmap);
	}

	private WriteableBitmap? _bitmap;

	private void UpdateSource(ScreenshotImageLoader? oldImageLoader = null)
	{
		RecycleBitmap(oldImageLoader);
		LoadImage();
	}

	private void RecycleBitmap(ScreenshotImageLoader? oldImageLoader)
	{
		oldImageLoader ??= ImageLoader;
		if (_bitmap != null)
		{
			Guard.IsNotNull(oldImageLoader);
			oldImageLoader.ReturnBitmapToPool(_bitmap);
		}
		_bitmap = null;
	}

	private void LoadImage()
	{
		if (AssociatedObject?.IsLoaded != true)
			return;
		if (Screenshot != null && ImageLoader != null)
		{
			var maximumLargestDimension = RoundSize(Math.Max(TargetSize.Width, TargetSize.Height));
			_bitmap = TargetSize != default
				? ImageLoader.LoadImage(Screenshot, maximumLargestDimension)
				: ImageLoader.LoadImage(Screenshot);
		}
		AssociatedObject.Source = _bitmap;
	}

	private int RoundSize(double size)
	{
		var rounded = (int)Math.Round(size / SizeStep) * SizeStep;
		if (rounded < MinimumSize)
			return MinimumSize;
		return rounded;
	}
}