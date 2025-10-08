namespace SightKeeper.Application.ScreenCapturing.Saving;

internal sealed class FuncImageSaverFactory<TPixel> : ImageSaverFactory<TPixel>
{
	public FuncImageSaverFactory(Func<ImageSaver<TPixel>> func)
	{
		_func = func;
	}

	public ImageSaver<TPixel> CreateImageSaver()
	{
		return _func();
	}

	private readonly Func<ImageSaver<TPixel>> _func;
}