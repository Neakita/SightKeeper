namespace SightKeeper.Application.Training;

public interface CommandRunner
{
	Task ExecuteCommandAsync(string command);
}