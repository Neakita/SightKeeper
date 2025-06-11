using FluentAssertions;

namespace SightKeeper.Data.Tests.Saving;

public sealed class ImageSetsSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Utilities.CreateImageSet();
		set.Name = name;
		var persistedSet = set.Persist();
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Utilities.CreateImageSet();
		set.Description = description;
		var persistedSet = set.Persist();
		persistedSet.Description.Should().Be(description);
	}
}