using FluentAssertions;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Domain.Tests.Profiles.Modules.Scaling;

public sealed class AdaptiveScalingOptionsTests
{
	[Fact]
	public void InitialValuesShouldBeValid()
	{
		AdaptiveScalingOptions options = new();
		options.Margin = options.Margin;
		options.MaximumScaling = options.MaximumScaling;
	}

	[Fact]
	public void ShouldSetMarginToZero()
	{
		AdaptiveScalingOptions options = new();
		options.Margin = 0;
		options.Margin.Should().Be(0);
	}

	[Fact]
	public void ShouldSetMarginToJustGreaterThanZero()
	{
		AdaptiveScalingOptions options = new();
		options.Margin = 0.01f;
		options.Margin.Should().Be(0.01f);
	}

	[Fact]
	public void ShouldNotSetMarginToNegativeValue()
	{
		AdaptiveScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.Margin = -1);
		options.Margin.Should().NotBe(-1);
	}

	[Fact]
	public void ShouldSetMaxScalingToJustGreaterThanOne()
	{
		AdaptiveScalingOptions options = new();
		options.MaximumScaling = 1.01f;
		options.MaximumScaling.Should().Be(1.01f);
	}

	[Fact]
	public void ShouldNotSetMaxScalingToOne()
	{
		AdaptiveScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.MaximumScaling = 1);
		options.MaximumScaling.Should().NotBe(1);
	}
}