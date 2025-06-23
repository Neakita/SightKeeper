using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Assets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ImageAssetsObservingTests
{
	[Fact]
	public void ShouldObserveAddition()
	{
		var image = Utilities.CreateImage();
		var asset = Substitute.For<Asset>();
		var observableAssets = image.Assets.Should().BeAssignableTo<ReadOnlyObservableCollection<Asset>>().Subject;
		List<Change<Asset>> assets = new();
		observableAssets.Subscribe(assets.Add);
		image.AddAsset(asset);
		assets.Should()
			.ContainSingle()
			.Which.Should().BeOfType<Addition<Asset>>()
			.Which.Items.Should().ContainSingle()
			.Which.Should().BeSameAs(asset);
	}
}