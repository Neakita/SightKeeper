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

	public static readonly StyledProperty<int> TargetSizeProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, int>(nameof(TargetSize));

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

	public int TargetSize
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
			UpdateSource(oldImageLoader);
			return;
		}
		UpdateSource();
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
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

	private void RecycleBitmap(ScreenshotImageLoader? imageLoader)
	{
		imageLoader ??= ImageLoader;
		if (_bitmap != null)
		{
			Guard.IsNotNull(imageLoader);
			imageLoader.ReturnBitmapToPool(_bitmap);
		}
		_bitmap = null;
	}

	private void LoadImage()
	{
		if (AssociatedObject?.IsLoaded != true ||
		    Screenshot == null ||
		    ImageLoader == null)
			return;
		_bitmap = TargetSize == 0
			? ImageLoader.LoadImage(Screenshot)
			: ImageLoader.LoadImage(Screenshot, TargetSize);
		AssociatedObject.Source = null; // TODO some kind of bug in the framework, because of which Image control does not update the rendering when manually assigning the source under certain conditions
		AssociatedObject.Source = _bitmap;
	}
}