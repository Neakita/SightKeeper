using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class ScreenshotViewModel : ViewModel
{
	public Screenshot Screenshot { get; }
	public WriteableBitmap Image => _imageLoader.LoadImage(Screenshot);

	public WriteableBitmap LoadImage(int maximumLargestDimension)
	{
		return _imageLoader.LoadImage(Screenshot, maximumLargestDimension);
	}

	public ScreenshotViewModel(Screenshot screenshot, ScreenshotImageLoader imageLoader)
	{
		Screenshot = screenshot;
		_imageLoader = imageLoader;
	}

	private readonly ScreenshotImageLoader _imageLoader;

	[RelayCommand]
	private void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_imageLoader.ReturnBitmapToPool(bitmap);
	}
}