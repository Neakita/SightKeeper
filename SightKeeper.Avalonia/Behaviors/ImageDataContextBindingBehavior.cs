using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class ImageDataContextBindingBehavior : Behavior<Image>
{
	public static readonly StyledProperty<ImageDataContext?> DataContextProperty =
		AvaloniaProperty.Register<ImageDataContextBindingBehavior, ImageDataContext?>(nameof(DataContext));

	public static readonly StyledProperty<int?> TargetSizeProperty =
		AvaloniaProperty.Register<ImageDataContextBindingBehavior, int?>(nameof(TargetSize));

	public ImageDataContext? DataContext
	{
		get => GetValue(DataContextProperty);
		set => SetValue(DataContextProperty, value);
	}

	public int? TargetSize
	{
		get => GetValue(TargetSizeProperty);
		set => SetValue(TargetSizeProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
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
		RecycleBitmap();
	}

	private static readonly ILogger Logger = Log.ForContext<ImageDataContextBindingBehavior>();
	private Bitmap? _bitmap;
	private CancellationTokenSource? _cancellationTokenSource;

	private async void UpdateSourceAsyncOrThrow()
	{
		if (_cancellationTokenSource != null)
		{
			await _cancellationTokenSource.CancelAsync();
			_cancellationTokenSource = null;
		}
		_cancellationTokenSource = new CancellationTokenSource();
		await UpdateSourceAsync(_cancellationTokenSource.Token);
	}

	private Task UpdateSourceAsync(CancellationToken cancellationToken)
	{
		RecycleBitmap();
		return LoadImageAsync(cancellationToken);
	}

	private void RecycleBitmap()
	{
		if (_bitmap is PooledWriteableBitmap pooledWriteableBitmap)
			pooledWriteableBitmap.ReturnToPool();
		_bitmap = null;
	}

	private async Task LoadImageAsync(CancellationToken cancellationToken)
	{
		if (AssociatedObject?.IsLoaded != true || DataContext == null)
		{
			if (AssociatedObject != null)
				AssociatedObject.Source = null;
			return;
		}
		try
		{
			_bitmap = await DataContext.Load(TargetSize, cancellationToken);
		}
		catch (FileNotFoundException exception)
		{
			Logger.Warning(exception, "The image file was not found. It may have been deleted or moved by the user, or an error occurred while writing it.");
		}
		catch (IOException exception)
		{
			Logger.Warning(exception, "An error occurred while reading the image file. The file may still be being written.");
		}
		if (AssociatedObject == null)
		{
			if (_bitmap != null)
				RecycleBitmap();
			return;
		}
		AssociatedObject.Source = _bitmap;
		AssociatedObject.InvalidateVisual();
	}
}