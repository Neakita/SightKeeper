using System.Globalization;
using SightKeeper.Avalonia.Misc.Converters;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.Tests.Converters;

/// <summary>
/// In Avalonia fluent style three state checkbox with false value looks empty,
/// with null looks like "Stop" icon (square) and with true looks like check,
/// so if condition is "IsNotShooting", checkbox should have null value for "Stop" like looking
/// </summary>
public sealed class ItemClassActivationConditionToBoolConverterTests
{
    [Theory]
    [InlineData(ActivationCondition.None, false)]
    [InlineData(ActivationCondition.IsShooting, true)]
    [InlineData(ActivationCondition.IsNotShooting, null)]
    public void ShouldProperlyConvert(ActivationCondition itemClassActivationCondition, bool? expected)
    {
        ItemClassActivationConditionToBoolConverter converter = new();
        var convertResult = converter.Convert(itemClassActivationCondition, typeof(bool?), null, CultureInfo.InvariantCulture);
        convertResult.Should().Be(expected);
    }

    [Theory]
    [InlineData(true, ActivationCondition.IsShooting)]
    [InlineData(false, ActivationCondition.None)]
    [InlineData(null, ActivationCondition.IsNotShooting)]
    public void ShouldProperlyConvertBack(bool? value, ActivationCondition expected)
    {
        ItemClassActivationConditionToBoolConverter converter = new();
        var convertResult = converter.ConvertBack(value, typeof(ActivationCondition), null, CultureInfo.InvariantCulture);
        convertResult.Should().Be(expected);
    }

    [Theory]
    [InlineData("None, lol", typeof(bool?))]
    [InlineData(ActivationCondition.None, typeof(int))]
    public void ShouldThrowArgumentExceptionWhenConverting(object value, Type targetType)
    {
        ItemClassActivationConditionToBoolConverter converter = new();
        Action action = () => converter.Convert(value, targetType, null, CultureInfo.InvariantCulture);
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("None, lol", typeof(ActivationCondition))]
    [InlineData(true, typeof(int))]
    public void ShouldThrowArgumentExceptionWhenConvertingBack(object value, Type targetType)
    {
        ItemClassActivationConditionToBoolConverter converter = new();
        Action action = () => converter.ConvertBack(value, targetType, null, CultureInfo.InvariantCulture);
        action.Should().Throw<ArgumentException>();
    }
}