/*using FluentAssertions;
using SightKeeper.Application.Training.Assets.Distribution;

namespace SightKeeper.Application.Tests.Training;

public sealed class AssetsDistributorTests
{
	[Fact]
	public void ShouldDistributeAssets()
	{
		var assets = Enumerable.Range(0, 100).Select(_ => new FakeAsset
		{
			Image = null!
		}).ToList();
		AssetsDistributor distributor = new()
		{
			Assets = assets,
			TrainFraction = 0.8,
			ValidationFraction = 0.15,
			TestFraction = 0.05
		};
		distributor.TrainAssets.Count.Should().Be(80);
		distributor.ValidationAssets.Count.Should().Be(15);
		distributor.TestAssets.Count.Should().Be(5);
	}

	[Fact]
	public void ShouldDistributeAssetsWhenFractionsSumIsNotOne()
	{
		var assets = Enumerable.Range(0, 100).Select(_ => new FakeAsset
		{
			Image = null!
		}).ToList();
		AssetsDistributor distributor = new()
		{
			Assets = assets,
			TrainFraction = 80,
			ValidationFraction = 15,
			TestFraction = 5
		};
		distributor.TrainAssets.Count.Should().Be(80);
		distributor.ValidationAssets.Count.Should().Be(15);
		distributor.TestAssets.Count.Should().Be(5);
	}
}*/