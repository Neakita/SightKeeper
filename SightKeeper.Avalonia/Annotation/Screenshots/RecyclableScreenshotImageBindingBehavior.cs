using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class RecyclableScreenshotImageBindingBehavior : Behavior<Image>
{
	public static readonly StyledProperty<Screenshot?> ScreenshotProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, Screenshot?>(nameof(Screenshot));

	public static readonly StyledProperty<ScreenshotImageLoader?> ImageLoaderProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, ScreenshotImageLoader?>(nameof(ImageLoader));

	public static readonly StyledProperty<int?> TargetSizeProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, int?>(nameof(TargetSize));

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

	public int? TargetSize
	{
		get => GetValue(TargetSizeProperty);
		set => SetValue(TargetSizeProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ImageLoaderProperty)
		{
			var oldImageLoader = change.GetOldValue<ScreenshotImageLoader?>();
			UpdateSourceAsyncOrThrow(oldImageLoader);
			return;
		}
		UpdateSourceAsyncOrThrow();
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

	private async void UpdateSourceAsyncOrThrow(ScreenshotImageLoader? imageLoader = null)
	{
		if (_cancellationTokenSource != null)
		{
			await _cancellationTokenSource.CancelAsync();
			_cancellationTokenSource = null;
		}
		_cancellationTokenSource = new CancellationTokenSource();
		await UpdateSourceAsync(imageLoader, _cancellationTokenSource.Token);
	}

	private Task UpdateSourceAsync(ScreenshotImageLoader? oldImageLoader, CancellationToken cancellationToken)
	{
		RecycleBitmap(oldImageLoader);
		return LoadImageAsync(cancellationToken);
	}

	private void RecycleBitmap(ScreenshotImageLoader? imageLoader = null)
	{
		if (_bitmap == null)
			return;
		imageLoader ??= ImageLoader;
		Guard.IsNotNull(imageLoader);
		imageLoader.ReturnBitmapToPool(_bitmap);
		_bitmap = null;
	}

	private async Task LoadImageAsync(CancellationToken cancellationToken)
	{
		if (AssociatedObject?.IsLoaded != true ||
		    Screenshot == null ||
		    ImageLoader == null)
		{
			if (AssociatedObject != null)
				AssociatedObject.Source = null;
			return;
		}
		_bitmap = await ImageLoader.LoadImageAsync(Screenshot, TargetSize, cancellationToken);
		if (AssociatedObject == null)
		{
			if (_bitmap != null)
				RecycleBitmap();
			return;
		}
		AssociatedObject.Source = null; // TODO some kind of bug in the framework, because of which Image control does not update the rendering when manually assigning the source under certain conditions
		AssociatedObject.Source = _bitmap;
	}
}