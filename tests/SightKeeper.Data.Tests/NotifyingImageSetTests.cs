using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Decorators;

namespace SightKeeper.Data.Tests;

public sealed class NotifyingImageSetTests
{
	[Fact]
	public void ShouldObserveNameChange()
	{
		var set = new NotifyingImageSet(Substitute.For<StorableImageSet>());
		using var monitoredSet = set.Monitor();
		set.Name = "New name";
		monitoredSet.Should().RaisePropertyChangeFor(imageSet => imageSet.Name);
	}

	[Fact]
	public void ShouldObserveDescriptionChange()
	{
		var set = new NotifyingImageSet(Substitute.For<StorableImageSet>());
		using var monitoredSet = set.Monitor();
		set.Description = "New description";
		monitoredSet.Should().RaisePropertyChangeFor(imageSet => imageSet.Description);
	}
}