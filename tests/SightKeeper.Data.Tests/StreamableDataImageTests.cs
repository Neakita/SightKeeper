using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain;

namespace SightKeeper.Data.Tests;

public sealed class StreamableDataImageTests
{
	[Fact]
	public void ShouldReturnWriteStreamFromDataAccess()
	{
		var dataAccess = Substitute.For<FileSystemDataAccess>();
		using MemoryStream stream = new();
		dataAccess.OpenWrite(Arg.Any<Id>()).Returns(stream);
		var inMemoryImage = new InMemoryImage(Id.Create(), DateTimeOffset.UtcNow, new Vector2<ushort>());
		StreamableDataImage image = new(inMemoryImage, dataAccess);
		var streamFromImage = image.OpenWriteStream();
		streamFromImage.Should().BeSameAs(stream);
	}

	[Fact]
	public void ShouldReturnReadStreamFromDataAccess()
	{
		var dataAccess = Substitute.For<FileSystemDataAccess>();
		using MemoryStream stream = new();
		dataAccess.OpenRead(Arg.Any<Id>()).Returns(stream);
		var inMemoryImage = new InMemoryImage(Id.Create(), DateTimeOffset.UtcNow, new Vector2<ushort>());
		StreamableDataImage image = new(inMemoryImage, dataAccess);
		var streamFromImage = image.OpenReadStream();
		streamFromImage.Should().BeSameAs(stream);
	}
}