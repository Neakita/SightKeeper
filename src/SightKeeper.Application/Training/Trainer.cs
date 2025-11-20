namespace SightKeeper.Application.Training;

public interface Trainer
{
	Task TrainAsync(CancellationToken cancellationToken);
}