using FluentAssertions;
using SightKeeper.Application.Training.Assets.Distribution;

namespace SightKeeper.Application.Tests.Training;

public sealed class AssetsDistributionRequestTests
{
	[Fact]
	public void ShouldSummariseNormalizedFractions()
	{
		var request = new AssetsDistributionRequest
		{
			TrainFraction = .8f,
			ValidationFraction = .15f,
			TestFraction = .05f
		};
		request.FractionsSum.Should().BeApproximately(1, float.Epsilon);
	}

	[Fact]
	public void ShouldSummariseNonNormalizedFractions()
	{
		var request = new AssetsDistributionRequest
		{
			TrainFraction = 15,
			ValidationFraction = 2,
			TestFraction = 0.5f
		};
		request.FractionsSum.Should().BeApproximately(17.5f, float.Epsilon);
	}

	[Fact]
	public void ShouldNormalizeFractions()
	{
		var request = new AssetsDistributionRequest
		{
			TrainFraction = 80,
			ValidationFraction = 15,
			TestFraction = 5
		};
		request = request.Normalized;
		request.TrainFraction.Should().BeApproximately(.8f, float.Epsilon);
		request.ValidationFraction.Should().BeApproximately(.15f, float.Epsilon);
		request.TestFraction.Should().BeApproximately(.05f, float.Epsilon);
	}
}