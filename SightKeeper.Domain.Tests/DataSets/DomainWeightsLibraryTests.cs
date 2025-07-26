using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainWeightsLibraryTests
{
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