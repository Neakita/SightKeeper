namespace SightKeeper.Application.ScreenCapturing;

public interface UnusedImagesLimitManager
{
	ushort? UnusedImagesLimit { get; set; }
}