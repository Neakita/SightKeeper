using System;
using System.Threading;
using System.Threading.Tasks;
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
			UpdateSourceAsyncOrThrow();
		else
		{
			var oldImageLoader = change.GetOldValue<ScreenshotImageLoader?>();
			UpdateSourceAsyncOrThrow(oldImageLoader);
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
		UpdateSourceAsyncOrThrow();
	}

	protected override void OnDetachedFromVisualTree()
	{
		_cancellationTokenSource?.Cancel();
		if (_bitmap == null)
			return;
		Guard.IsNotNull(ImageLoader);
		ImageLoader.ReturnBitmapToPool(_bitmap);
	}

	private WriteableBitmap? _bitmap;
	private CancellationTokenSource? _cancellationTokenSource;

	private async void UpdateSourceAsyncOrThrow(ScreenshotImageLoader? oldImageLoader = null)
	{
		try
		{
			await UpdateSourceAsync(oldImageLoader);
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
			throw;
		}
	}

	private async Task UpdateSourceAsync(ScreenshotImageLoader? oldImageLoader = null)
	{
		RecycleBitmap(oldImageLoader);
		if (AssociatedObject?.IsLoaded != true)
			return;
		if (_cancellationTokenSource != null)
		{
			await _cancellationTokenSource.CancelAsync();
			_cancellationTokenSource = null;
		}
		if (Screenshot != null && ImageLoader != null)
		{
			Guard.IsNull(_cancellationTokenSource);
			_cancellationTokenSource = new CancellationTokenSource();
			if (TargetSize == default)
				_bitmap = await ImageLoader.LoadImageAsync(Screenshot, _cancellationTokenSource.Token);
			else
			{
				var maximumLargestDimension = RoundSize(Math.Max(TargetSize.Width, TargetSize.Height));
				_bitmap = await ImageLoader.LoadImageAsync(Screenshot, maximumLargestDimension, _cancellationTokenSource.Token);
			}
			_cancellationTokenSource = null;
		}

		if (AssociatedObject == null)
		{
			if (_bitmap != null)
			{
				Guard.IsNotNull(ImageLoader);
				ImageLoader.ReturnBitmapToPool(_bitmap);
				_bitmap = null;
			}
		}
		else
		{
			AssociatedObject.Source = null; // image doesn't update under some circumstances without it
			AssociatedObject.Source = _bitmap;
		}
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

	private int RoundSize(double size)
	{
		var rounded = (int)Math.Round(size / SizeStep) * SizeStep;
		return Math.Max(rounded, MinimumSize);
	}
}