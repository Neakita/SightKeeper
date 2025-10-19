using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ObservableAssetsImageTests
{
	[Fact]
	public void ShouldObserveAddition()
	{
		var image = new ObservableAssetsImage(Substitute.For<ManagedImage, EditableImageAssets>());
		var asset = Substitute.For<Asset>();
		var observableAssets = image.Assets.Should().BeAssignableTo<ReadOnlyObservableCollection<Asset>>().Subject;
		List<Change<Asset>> observedChanges = new();
		observableAssets.Subscribe(observedChanges.Add);
		image.Add(asset);
		observedChanges.Should()
			.ContainSingle()
			.Which.Should().BeOfType<Addition<Asset>>()
			.Which.Items.Should().ContainSingle()
			.Which.Should().BeSameAs(asset);
	}

	[Fact]
	public void ShouldObserveRemoval()
	{
		var image = new ObservableAssetsImage(Substitute.For<ManagedImage, EditableImageAssets>());
		var asset = Substitute.For<Asset>();
		var observableAssets = image.Assets.Should().BeAssignableTo<ReadOnlyObservableCollection<Asset>>().Subject;
		List<Change<Asset>> observedChanges = new();
		observableAssets.Subscribe(observedChanges.Add);
		image.Remove(asset);
		observedChanges.Should()
			.ContainSingle()
			.Which.Should().BeOfType<Removal<Asset>>()
			.Which.Items.Should().ContainSingle()
			.Which.Should().BeSameAs(asset);
	}

	[Fact]
	public void ShouldPropagateAddition()
	{
		var innerImage = Substitute.For<ManagedImage, EditableImageAssets>();
		var image = new ObservableAssetsImage(innerImage);
		var asset = Substitute.For<Asset>();
		image.Add(asset);
		var editableAssetsImage = (EditableImageAssets)innerImage;
		editableAssetsImage.Received().Add(asset);
	}

	[Fact]
	public void ShouldPropagateRemoval()
	{
		var innerImage = Substitute.For<ManagedImage, EditableImageAssets>();
		var image = new ObservableAssetsImage(innerImage);
		var asset = Substitute.For<Asset>();
		image.Remove(asset);
		var editableAssetsImage = (EditableImageAssets)innerImage;
		editableAssetsImage.Received().Remove(asset);
	}
}