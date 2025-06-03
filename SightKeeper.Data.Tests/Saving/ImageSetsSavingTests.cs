using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Saving;

public sealed class ImageSetsSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		ImageSet set = new()
		{
			Name = name
		};
		var persistedSet = Persist(set);
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		ImageSet set = new()
		{
			Description = description
		};
		var persistedSet = Persist(set);
		persistedSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldPersistMultipleImages()
	{
		ImageSet set = new();
		var initialTimestamp = DateTimeOffset.UtcNow;
		var timestamps = Enumerable.Range(0, 5)
			.Select(i => initialTimestamp.AddMilliseconds(i))
			.ToList();
		foreach (var timestamp in timestamps)
			set.CreateImage(timestamp, new Vector2<ushort>(320, 320));
		var persistedSet = Persist(set);
		persistedSet.Images.Select(image => image.CreationTimestamp).Should().ContainInOrder(timestamps);
	}

	private static ImageSet Persist(ImageSet set)
	{
		AppDataAccess appDataAccess = new();
		Utilities.AddImageSetToAppData(set, appDataAccess);
		var persistedAppData = appDataAccess.Data.Persist();
		return persistedAppData.ImageSets.Single();
	}
}