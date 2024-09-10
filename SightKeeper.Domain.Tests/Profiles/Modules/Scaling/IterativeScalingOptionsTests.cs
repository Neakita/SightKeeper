using FluentAssertions;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Domain.Tests.Profiles.Modules.Scaling;

public sealed class IterativeScalingOptionsTests
{
	[Fact]
	public void InitialValuesShouldBeValid()
	{
		IterativeScalingOptions options = new();
		options.Initial = options.Initial;
		options.StepSize = options.StepSize;
		options.StepsCount = options.StepsCount;
	}

	[Fact]
	public void ShouldSetInitialScalingToOne()
	{
		IterativeScalingOptions options = new();
		options.Initial = 1;
		options.Initial.Should().Be(1);
	}
	
	[Fact]
	public void ShouldNotSetInitialScalingToZero()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.Initial = 0);
		options.Initial.Should().NotBe(0);
	}

	[Fact]
	public void ShouldNotSetScalingToJustLesserThanOne()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.Initial = 0.99f);
		options.Initial.Should().BeGreaterThanOrEqualTo(1);
	}

	[Fact]
	public void ShouldSetStepSizeToJustGreaterThanZero()
	{
		IterativeScalingOptions options = new();
		options.StepSize = 0.01f;
		options.StepSize.Should().Be(0.01f);
	}

	[Fact]
	public void ShouldNotSetStepSizeToZero()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.StepSize = 0);
		options.StepSize.Should().NotBe(0);
	}

	[Fact]
	public void ShouldSetStepsCountToTwo()
	{
		IterativeScalingOptions options = new();
		options.StepsCount = 2;
		options.StepsCount.Should().Be(2);
	}

	[Fact]
	public void ShouldNotSetStepsCountToOne()
	{
		IterativeScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.StepsCount = 1);
		options.StepsCount.Should().NotBe(1);
	}
}