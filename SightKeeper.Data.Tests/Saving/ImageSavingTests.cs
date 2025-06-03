using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Saving;

public sealed class ImageSavingTests
{
	[Fact]
	public void ShouldPersistCreationTimestamp()
	{
		var creationTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
		var image = CreateImage(creationTimestamp, new Vector2<ushort>(320, 320));
		var persistedImage = Persist(image);
		persistedImage.CreationTimestamp.Should().Be(creationTimestamp);
	}

	[Fact]
	public void ShouldPersistSize()
	{
		var size = new Vector2<ushort>(480, 320);
		var image = CreateImage(DateTimeOffset.UtcNow, size);
		var persistedImage = Persist(image);
		persistedImage.Size.Should().Be(size);
	}

	private static Image CreateImage(DateTimeOffset creationTimestamp, Vector2<ushort> size)
	{
		ImageSet set = new();
		var image = set.CreateImage(creationTimestamp, size);
		return image;
	}

	private static Image Persist(Image image)
	{
		var set = image.Set;
		Guard.IsEqualTo(set.Images.Count, 1);
		AppDataAccess appDataAccess = new();
		Utilities.AddImageSetToAppData(set, appDataAccess);
		var persistedAppData = appDataAccess.Data.Persist();
		return persistedAppData.ImageSets.Single().Images.Single();
	}
}