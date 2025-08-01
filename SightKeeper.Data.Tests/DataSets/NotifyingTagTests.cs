using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class NotifyingTagTests
{
	[Fact]
	public void ShouldRaisePropertyChangedForName()
	{
		var innerTag = Substitute.For<StorableTag>();
		var notifyingTag = new NotifyingTag(innerTag);
		using var monitor = notifyingTag.Monitor();
		notifyingTag.Name = "new name";
		monitor.Should().RaisePropertyChangeFor(tag => tag.Name);
	}

	[Fact]
	public void ShouldRaisePropertyChangedForColor()
	{
		var innerTag = Substitute.For<StorableTag>();
		var notifyingTag = new NotifyingTag(innerTag);
		using var monitor = notifyingTag.Monitor();
		notifyingTag.Color = 123;
		monitor.Should().RaisePropertyChangeFor(tag => tag.Color);
	}
}