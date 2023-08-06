using SightKeeper.Commons;

namespace SightKeeper.Common.Tests;

public sealed class NumberExtensionsCycleTests
{
    [Fact]
    public void ShouldJustIncrementValue()
    {
        const int value = 1;
        const int min = 0;
        const int max = 3;
        var newValue = value.Cycle(min, max);
        newValue.Should().Be(2);
    }

    [Fact]
    public void ShouldJustDecrementValue()
    {
        const int value = 1;
        const int min = 0;
        const int max = 3;
        var newValue = value.Cycle(min, max, true);
        newValue.Should().Be(min);
    }

    [Fact]
    public void ShouldJumpFromMaxToMinValue()
    {
        const int max = 2;
        const int min = 0;
        var newValue = max.Cycle(min, max);
        newValue.Should().Be(min);
    }

    [Fact]
    public void ShouldJumpFromMinToMaxValue()
    {
        const int min = 0;
        const int max = 2;
        var newValue = min.Cycle(min, max, true);
        newValue.Should().Be(max);
    }
}