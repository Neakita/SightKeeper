using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class BoundingTests
{
	[Fact]
	public void ShouldCreateBoundingFromPoints()
	{
		const double left = .1;
		const double top = .2;
		const double right = .3;
		const double bottom = .4;
		Vector2<double> topLeft = new(left, top);
		Vector2<double> bottomRight = new(right, bottom);
		var bounding = Bounding.FromPoints(topLeft, bottomRight);
		bounding.Left.Should().Be(left);
		bounding.Top.Should().Be(top);
		bounding.Right.Should().Be(right);
		bounding.Bottom.Should().Be(bottom);
	}

	[Fact]
	public void ShouldCreateBoundingFromWronglyOrderedPoints()
	{
		const double left = .1;
		const double top = .2;
		const double right = .3;
		const double bottom = .4;
		Vector2<double> bottomLeft = new(left, bottom);
		Vector2<double> topRight = new(right, top);
		var bounding = Bounding.FromPoints(bottomLeft, topRight);
		bounding.Left.Should().Be(left);
		bounding.Top.Should().Be(top);
		bounding.Right.Should().Be(right);
		bounding.Bottom.Should().Be(bottom);
	}
}