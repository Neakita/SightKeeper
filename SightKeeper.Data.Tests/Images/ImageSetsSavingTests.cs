using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImageSetsSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Substitute.For<ImageSet>();
		set.Name.Returns(name);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Substitute.For<ImageSet>();
		set.Description.Returns(description);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.Description.Should().Be(description);
	}

	private static ImageSetFormatter Formatter => new(new FakeImageSetWrapper(), new FakeImageWrapper());
}