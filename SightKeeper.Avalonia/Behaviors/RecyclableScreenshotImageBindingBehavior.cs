using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class RecyclableScreenshotImageBindingBehavior : Behavior<Image>
{
	public static readonly StyledProperty<Screenshot?> ScreenshotProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, Screenshot?>(nameof(Screenshot));

	public static readonly StyledProperty<int?> TargetSizeProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, int?>(nameof(TargetSize));

	public static readonly StyledProperty<Composition?> CompositionProperty =
		AvaloniaProperty.Register<RecyclableScreenshotImageBindingBehavior, Composition?>(nameof(Composition));

	public Screenshot? Screenshot
	{
		get => GetValue(ScreenshotProperty);
		set => SetValue(ScreenshotProperty, value);
	}

	public int? TargetSize
	{
		get => GetValue(TargetSizeProperty);
		set => SetValue(TargetSizeProperty, value);
	}

	public Composition? Composition
	{
		get => GetValue(CompositionProperty);
		set => SetValue(CompositionProperty, value);
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
		if (_bitmap == null)
			return;
		Guard.IsNotNull(Composition);
		Composition.ScreenshotImageLoader.ReturnBitmapToPool(_bitmap);
	}

	private WriteableBitmap? _bitmap;
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
		if (_bitmap == null)
			return;
		Guard.IsNotNull(Composition);
		Composition.ScreenshotImageLoader.ReturnBitmapToPool(_bitmap);
		_bitmap = null;
	}

	private async Task LoadImageAsync(CancellationToken cancellationToken)
	{
		if (AssociatedObject?.IsLoaded != true ||
		    Screenshot == null ||
		    Composition == null)
		{
			if (AssociatedObject != null)
				AssociatedObject.Source = null;
			return;
		}
		_bitmap = await Composition.ScreenshotImageLoader.LoadImageAsync(Screenshot, TargetSize, cancellationToken);
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