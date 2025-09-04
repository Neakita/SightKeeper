using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets.Decorators;
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
		ObservableImagesImageSet set = new(Substitute.For<ImageSet>());
		var observableList = (ReadOnlyObservableList<ManagedImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Insertion<ManagedImage>>().Subject;
		insertion.Index.Should().Be(0);
		insertion.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImageRemoval()
	{
		var image = Substitute.For<ManagedImage>();
		var innerSet = Substitute.For<ImageSet>();
		innerSet.Images.Returns([image]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<ManagedImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImageAt(0);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<ManagedImage>).Subject.As<IndexedRemoval<ManagedImage>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImagesRangeRemoval()
	{
		var image1 = Substitute.For<ManagedImage>();
		var image2 = Substitute.For<ManagedImage>();
		var image3 = Substitute.For<ManagedImage>();
		var innerSet = Substitute.For<ImageSet>();
		innerSet.Images.Returns([image1, image2, image3]);
		innerSet.GetImagesRange(0, 2).Returns([image1, image2]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<ManagedImage>)set.Images;
		List<IndexedChange<ManagedImage>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImagesRange(0, 2);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<ManagedImage>).Subject.As<IndexedRemoval<ManagedImage>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainInOrder(image1, image2);
	}
}