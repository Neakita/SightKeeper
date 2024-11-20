using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract partial class ScreenshotViewModel : ViewModel
{
	public abstract Screenshot Value { get; }
	public abstract AssetViewModel? Asset { get; }
	public WriteableBitmap Image => _imageLoader.LoadImage(Value);

	public WriteableBitmap LoadImage(int maximumLargestDimension)
	{
		return _imageLoader.LoadImage(Value, maximumLargestDimension);
	}

	public ScreenshotViewModel(ScreenshotImageLoader imageLoader)
	{
		_imageLoader = imageLoader;
	}

	private readonly ScreenshotImageLoader _imageLoader;

	[RelayCommand]
	private void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_imageLoader.ReturnBitmapToPool(bitmap);
	}
}