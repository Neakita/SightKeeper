using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.Tests.DataSets.Screenshots;

public sealed class FixedTransparentImageCompositionTests
{
	[Fact]
	public void ShouldCreate()
	{
		FixedTransparentImageComposition _ = new(TimeSpan.FromMilliseconds(50), [0.2f, 0.3f, 0.5f]);
	}

	[Fact]
	public void ShouldNotCreateWithNegativeDelay()
	{
		Assert.ThrowsAny<Exception>(() => new FixedTransparentImageComposition(TimeSpan.FromMilliseconds(-1), [0.5f, 0.5f]));
	}

	[Fact]
	public void ShouldNotCreateWithOnlyOneOpacity()
	{
		Assert.ThrowsAny<Exception>(() => new FixedTransparentImageComposition(TimeSpan.FromMilliseconds(50), [1]));
	}

	[Fact]
	public void ShouldNotCreateWithOpacitiesSumNotEqualToOne()
	{
		Assert.ThrowsAny<Exception>(() => new FixedTransparentImageComposition(TimeSpan.FromMilliseconds(50), [0.1f, 0.3f, 0.5f]));
	}
}