using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ImageAssetsObservingTests
{
	[Fact]
	public void ShouldObserveAddition()
	{
		var image = new ObservableAssetsImage(Substitute.For<Image>());
		var asset = Substitute.For<Asset>();
		var observableAssets = image.Assets.Should().BeAssignableTo<ReadOnlyObservableCollection<Asset>>().Subject;
		List<Change<Asset>> observedChanges = new();
		observableAssets.Subscribe(observedChanges.Add);
		image.AddAsset(asset);
		observedChanges.Should()
			.ContainSingle()
			.Which.Should().BeOfType<Addition<Asset>>()
			.Which.Items.Should().ContainSingle()
			.Which.Should().BeSameAs(asset);
	}
}