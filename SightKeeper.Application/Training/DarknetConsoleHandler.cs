namespace SightKeeper.Application.Training;

public interface DarknetConsoleHandler
{
	event Action<BatchInfo>? BatchPassed;
}