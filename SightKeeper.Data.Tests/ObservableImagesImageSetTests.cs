using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
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
		var observableList = (ReadOnlyObservableList<Image>)set.Images;
		List<IndexedChange<Image>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var insertion = observedChanges.Should().ContainSingle().Which.Should().BeOfType<Insertion<Image>>().Subject;
		insertion.Index.Should().Be(0);
		insertion.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImageRemoval()
	{
		var image = Substitute.For<Image>();
		var innerSet = Substitute.For<ImageSet>();
		innerSet.Images.Returns([image]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<Image>)set.Images;
		List<IndexedChange<Image>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImageAt(0);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<Image>).Subject.As<IndexedRemoval<Image>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainSingle().Which.Should().Be(image);
	}

	[Fact]
	public void ShouldObserveImagesRangeRemoval()
	{
		var image1 = Substitute.For<Image>();
		var image2 = Substitute.For<Image>();
		var image3 = Substitute.For<Image>();
		var innerSet = Substitute.For<ImageSet>();
		innerSet.Images.Returns([image1, image2, image3]);
		innerSet.GetImagesRange(0, 2).Returns([image1, image2]);
		ObservableImagesImageSet set = new(innerSet);
		var observableList = (ReadOnlyObservableList<Image>)set.Images;
		List<IndexedChange<Image>> observedChanges = new();
		using var subscription = observableList.Subscribe(observedChanges.Add);
		set.RemoveImagesRange(0, 2);
		var removal = observedChanges.Should().ContainSingle(change => change is IndexedRemoval<Image>).Subject.As<IndexedRemoval<Image>>();
		removal.Index.Should().Be(0);
		removal.Items.Should().ContainInOrder(image1, image2);
	}
}