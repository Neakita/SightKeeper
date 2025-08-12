using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainKeyPointTests
{
	[Fact]
	public void ShouldAllowUpdatePosition()
	{
		var innerKeyPoint = Substitute.For<KeyPoint>();
		var domainKeyPoint = new DomainKeyPoint(innerKeyPoint);
		var newPosition = new Vector2<double>(0.2, 0.3);
		domainKeyPoint.Position = newPosition;
		innerKeyPoint.Received().Position = newPosition;
	}

	[Fact]
	public void ShouldDisallowUpdatePositionToNotNormalized()
	{
		var innerKeyPoint = Substitute.For<KeyPoint>();
		var domainKeyPoint = new DomainKeyPoint(innerKeyPoint);
		var newPosition = new Vector2<double>(2, 3);
		var exception = Assert.Throws<KeyPointPositionConstraintException>(() => domainKeyPoint.Position = newPosition);
		exception.KeyPoint.Should().Be(domainKeyPoint);
		exception.Value.Should().Be(newPosition);
		innerKeyPoint.DidNotReceive().Position = Arg.Any<Vector2<double>>();
	}
}