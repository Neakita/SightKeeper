namespace SightKeeper.Backend;

public interface IStreamScreenshoter
{
	public event ImageHandler? Screenshoted;
	
	
	public bool IsRunning { get; set; }
	public void Start();
	public void Stop();
}
