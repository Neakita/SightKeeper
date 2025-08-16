namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class FuncImageSaverFactory<TPixel> : ImageSaverFactory<TPixel>
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