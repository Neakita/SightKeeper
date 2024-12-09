namespace SightKeeper.Domain.DataSets.Weights;

public abstract class WeightsLibrary
{
	public abstract IReadOnlyCollection<Weights> Weights { get; }
}