using FluentAssertions;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Domain.Tests.Profiles.Modules.Scaling;

public sealed class IterativeScalingOptionsTests
{
	[Fact]
	public void InitialValuesShouldBeValid()
	{
		IterativeScalingOptions options = new();
		options.MinimumScaling = options.MinimumScaling;
		options.MaximumScaling = options.MaximumScaling;
	}

	[Fact]
	public void ShouldSetMinimumScalingToOne()
	{
		IterativeScalingOptions options = new();
		options.MinimumScaling = 1;
		options.MinimumScaling.Should().Be(1);
	}
	
	[Fact]
	public void ShouldNotSetMinimumScalingToZero()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.MinimumScaling = 0);
		options.MinimumScaling.Should().NotBe(0);
	}

	[Fact]
	public void ShouldNotSetMinimumScalingToJustLesserThanOne()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.MinimumScaling = 0.99f);
		options.MinimumScaling.Should().NotBe(0.99f);
	}

	[Fact]
	public void ShouldSetMaximumScalingToJustGreaterThanOne()
	{
		IterativeScalingOptions options = new();
		options.MaximumScaling = 1.01f;
		options.MaximumScaling.Should().Be(1.01f);
	}

	[Fact]
	public void ShouldNotSetMaximumScalingToOne()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.MaximumScaling = 1);
		options.MaximumScaling.Should().NotBe(1);
	}
}