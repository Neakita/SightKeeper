using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using Image = SightKeeper.Domain.Images.Image;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class RecyclableImageBindingBehavior : Behavior<global::Avalonia.Controls.Image>
{
	public static readonly StyledProperty<Image?> ImageProperty =
		AvaloniaProperty.Register<RecyclableImageBindingBehavior, Image?>(nameof(Image));

	public static readonly StyledProperty<int?> TargetSizeProperty =
		AvaloniaProperty.Register<RecyclableImageBindingBehavior, int?>(nameof(TargetSize));

	public static readonly StyledProperty<Composition?> CompositionProperty =
		AvaloniaProperty.Register<RecyclableImageBindingBehavior, Composition?>(nameof(Composition));

	public Image? Image
	{
		get => GetValue(ImageProperty);
		set => SetValue(ImageProperty, value);
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
		Composition.ImageLoader.ReturnBitmapToPool(_bitmap);
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
		Composition.ImageLoader.ReturnBitmapToPool(_bitmap);
		_bitmap = null;
	}

	private async Task LoadImageAsync(CancellationToken cancellationToken)
	{
		if (AssociatedObject?.IsLoaded != true ||
		    Image == null ||
		    Composition == null)
		{
			if (AssociatedObject != null)
				AssociatedObject.Source = null;
			return;
		}
		_bitmap = await Composition.ImageLoader.LoadImageAsync(Image, TargetSize, cancellationToken);
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