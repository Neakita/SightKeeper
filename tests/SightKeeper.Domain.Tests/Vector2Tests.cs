using FluentAssertions;

namespace SightKeeper.Domain.Tests;

public sealed class Vector2Tests
{
	[Fact]
	public void ShouldAdd()
	{
		Vector2<int> x = new(1, 2);
		Vector2<int> y = new(3, 4);
		Vector2<int> expected = new(4, 6);
		(x + y).Should().Be(expected);
	}

	[Fact]
	public void ShouldSubtract()
	{
		Vector2<int> x = new(9, 10);
		Vector2<int> y = new(1, 3);
		Vector2<int> expected = new(8, 7);
		(x - y).Should().Be(expected);
	}

	[Fact]
	public void ShouldMultiply()
	{
		Vector2<int> x = new(2, 3);
		Vector2<int> y = new(4, 5);
		Vector2<int> expected = new(8, 15);
		(x * y).Should().Be(expected);
	}

	[Fact]
	public void ShouldMultiplyByNumber()
	{
		Vector2<int> x = new(2, 3);
		const int y = 4;
		Vector2<int> expected = new(8, 12);
		(x * y).Should().Be(expected);
	}

	[Fact]
	public void ShouldDivide()
	{
		Vector2<int> x = new(8, 15);
		Vector2<int> y = new(4, 5);
		Vector2<int> expected = new(2, 3);
		(x / y).Should().Be(expected);
	}

	[Fact]
	public void ShouldDivideByNumber()
	{
		Vector2<int> x = new(8, 12);
		const int y = 4;
		Vector2<int> expected = new(2, 3);
		(x / y).Should().Be(expected);
	}
}