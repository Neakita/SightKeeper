namespace SightKeeper.Application.Training;

public sealed class Progress
{
	public string Label { get; init; } = string.Empty;
	public int Total { get; init; }
	public int Current { get; init; }
	public DateTime? EstimatedTimeOfArrival { get; init; }

	public double Percentage => (double)Current / Total;
}