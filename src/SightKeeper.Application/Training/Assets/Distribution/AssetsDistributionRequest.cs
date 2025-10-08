namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class AssetsDistributionRequest
{
	public float FractionsSum => TrainFraction + ValidationFraction + TestFraction;
	public float TrainFraction { get; init; }
	public float ValidationFraction { get; init; }
	public float TestFraction { get; init; }

	public AssetsDistributionRequest Normalized => new()
	{
		TrainFraction = TrainFraction / FractionsSum,
		ValidationFraction = ValidationFraction / FractionsSum,
		TestFraction = TestFraction / FractionsSum
	};
}