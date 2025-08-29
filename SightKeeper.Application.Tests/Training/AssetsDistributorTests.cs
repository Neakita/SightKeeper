using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Tests.Training;

public sealed class AssetsDistributorTests
{
	[Fact]
	public void ShouldDistributeAssetsWithNormalizedRequest()
	{
		var asset = Substitute.For<AssetData>();
		asset.Usage.Returns(AssetUsage.Any);
		var assets = Enumerable.Repeat(asset, 100);
		var request = new AssetsDistributionRequest
		{
			TrainFraction = .8f,
			ValidationFraction = .15f,
			TestFraction = .05f
		};
		var distribution = AssetsDistributor.DistributeAssets(assets, request);
		distribution.TrainAssets.Count.Should().Be(80);
		distribution.ValidationAssets.Count.Should().Be(15);
		distribution.TestAssets.Count.Should().Be(5);
	}

	[Fact]
	public void ShouldDistributeAssetsWithNonNormalizedRequest()
	{
		var asset = Substitute.For<AssetData>();
		asset.Usage.Returns(AssetUsage.Any);
		var assets = Enumerable.Repeat(asset, 100);
		var request = new AssetsDistributionRequest
		{
			TrainFraction = 8,
			ValidationFraction = 1.5f,
			TestFraction = 0.5f
		};
		var distribution = AssetsDistributor.DistributeAssets(assets, request);
		distribution.TrainAssets.Count.Should().Be(80);
		distribution.ValidationAssets.Count.Should().Be(15);
		distribution.TestAssets.Count.Should().Be(5);
	}
}