using System.Collections.ObjectModel;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainWeightsLibraryTests
{
	[Fact]
	public void ShouldAllowWeightsCreationWhenTagsSatisfiesMinimumTagsCount()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<Tag>>();
		var library = new DomainWeightsLibrary(
			innerLibrary,
			tagsOwner,
			2);
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(tagsOwner);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(tagsOwner);
		IReadOnlyCollection<Tag> tags = [tag1, tag2];
		library.CreateWeights(new WeightsMetadata(), tags);
		innerLibrary.Received().CreateWeights(Arg.Any<WeightsMetadata>(), tags);
	}

	[Fact]
	public void ShouldDisallowWeightsCreationWhenTagsDoesNotSatisfiesMinimumTagsCount()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<Tag>>();
		var library = new DomainWeightsLibrary(
			innerLibrary,
			tagsOwner,
			2);
		var tag = Substitute.For<Tag>();
		tag.Owner.Returns(tagsOwner);
		IReadOnlyCollection<Tag> tags = [tag];
		Assert.Throws<ArgumentException>(() => library.CreateWeights(new WeightsMetadata(), tags));
		innerLibrary.DidNotReceive().CreateWeights(Arg.Any<WeightsMetadata>(), Arg.Any<IReadOnlyCollection<Tag>>());
	}

	[Fact]
	public void ShouldSendWeightsMetadataToInnerLibraryWhenCreatingWeights()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<Tag>>();
		var library = new DomainWeightsLibrary(
			innerLibrary,
			tagsOwner,
			0);
		var weightsMetadata = new WeightsMetadata
		{
			Model = Model.UltralyticsYoloV11,
			CreationTimestamp = DateTimeOffset.Now,
			ModelSize = ModelSize.Small,
			Metrics = new WeightsMetrics(1, new LossMetrics(2, 3, 4)),
			Resolution = new Vector2<ushort>(480, 640)
		};
		library.CreateWeights(weightsMetadata, ReadOnlyCollection<Tag>.Empty);
		innerLibrary.Received().CreateWeights(weightsMetadata, Arg.Any<IReadOnlyCollection<Tag>>());
	}

	[Fact]
	public void ShouldGetWeightsFromInnerLibrary()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var weights = Substitute.For<Weights>();
		innerLibrary.Weights.Returns([weights]);
		var tagsOwner = Substitute.For<TagsOwner<PoserTag>>();
		DomainWeightsLibrary library = new(innerLibrary, tagsOwner);
		library.Weights.Should().HaveCount(1).And.Contain(weights);
	}

	[Fact]
	public void ShouldRemoveWeightsInInnerLibrary()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<PoserTag>>();
		DomainWeightsLibrary library = new(innerLibrary, tagsOwner);
		var weights = Substitute.For<Weights>();
		library.RemoveWeights(weights);
		innerLibrary.Received().RemoveWeights(weights);
	}

	[Fact]
	public void ShouldAllowKeyPointTagWithPoserTag()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<PoserTag>>();
		DomainWeightsLibrary library = new(innerLibrary, tagsOwner);
		var poserTag = Substitute.For<PoserTag>();
		poserTag.Owner.Returns(tagsOwner);
		((Tag)poserTag).Owner.Returns(tagsOwner);
		var keyPointTag = Substitute.For<Tag>();
		keyPointTag.Owner.Returns(poserTag);
		IReadOnlyCollection<Tag> tags = [poserTag, keyPointTag];
		library.CreateWeights(new WeightsMetadata(), tags);
		innerLibrary.Received().CreateWeights(Arg.Any<WeightsMetadata>(), tags);
	}

	[Fact]
	public void ShouldDisallowKeyPointTagWithoutPoserTag()
	{
		var innerLibrary = Substitute.For<WeightsLibrary>();
		var tagsOwner = Substitute.For<TagsOwner<PoserTag>>();
		DomainWeightsLibrary library = new(innerLibrary, tagsOwner);
		var poserTag = Substitute.For<PoserTag>();
		poserTag.Owner.Returns(tagsOwner);
		((Tag)poserTag).Owner.Returns(tagsOwner);
		var keyPointTag = Substitute.For<Tag>();
		keyPointTag.Owner.Returns(poserTag);
		Assert.Throws<KeyPointTagWithoutOwnerException>(() => library.CreateWeights(new WeightsMetadata(), [keyPointTag]));
		innerLibrary.DidNotReceive().CreateWeights(Arg.Any<WeightsMetadata>(), Arg.Any<IReadOnlyCollection<Tag>>());
	}
}