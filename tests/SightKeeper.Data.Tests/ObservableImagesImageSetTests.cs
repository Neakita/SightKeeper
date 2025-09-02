using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Data.Tests;

public sealed class ObservableImagesImageSetTests
{
	[Fact]
	public void ShouldObserveImageCreation()
	{
		ObservableImagesImageSet set = new(Substitute.For<StorableImageSet>());
		var observableList = (ReadOnlyObservableList<StorableImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Insertion<StorableImage>>().Subject;
		insertion.Index.Should().Be(0);
		insertion.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImageRemoval()
	{
		var image = Substitute.For<StorableImage>();
		var innerSet = Substitute.For<StorableImageSet>();
		innerSet.Images.Returns([image]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<StorableImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImageAt(0);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<StorableImage>).Subject.As<IndexedRemoval<StorableImage>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImagesRangeRemoval()
	{
		var image1 = Substitute.For<StorableImage>();
		var image2 = Substitute.For<StorableImage>();
		var image3 = Substitute.For<StorableImage>();
		var innerSet = Substitute.For<StorableImageSet>();
		innerSet.Images.Returns([image1, image2, image3]);
		innerSet.GetImagesRange(0, 2).Returns([image1, image2]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<StorableImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImagesRange(0, 2);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<StorableImage>).Subject.As<IndexedRemoval<StorableImage>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainInOrder(image1, image2);
	}
}