using FluentAssertions;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Domain.Tests.Profiles.Modules.Scaling;

public sealed class ConstantScalingOptionsTests
{
	[Fact]
	public void InitialFactorShouldBeValid()
	{
		ConstantScalingOptions options = new();
		options.Factor = options.Factor;
	}

	[Fact]
	public void ShouldSetMaxScalingToJustGreaterThanOne()
	{
		ConstantScalingOptions options = new();
		options.Factor = 1.01f;
		options.Factor.Should().Be(1.01f);
	}
	
	[Fact]
	public void ShouldNotSetMaxScalingToOne()
	{
		ConstantScalingOptions options = new();
		Assert.ThrowsAny<Exception>(() => options.Factor = 1);
		options.Factor.Should().NotBe(1);
	}
}