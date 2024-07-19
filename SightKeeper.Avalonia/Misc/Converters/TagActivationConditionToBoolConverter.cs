using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Serilog;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class TagActivationConditionToBoolConverter : IValueConverter
{
    public TagActivationConditionToBoolConverter()
    {
        _logger = Log.ForContext<TagActivationConditionToBoolConverter>();
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
	    throw new NotImplementedException();
	    // if (value == null)
	    //     return null;
	    // if (value is not ActivationCondition tagActivationCondition)
	    //     return ThrowHelper.ThrowArgumentException<object>(nameof(value), $"Expected an \"{typeof(ActivationCondition)}\", but got \"{value}\" of type \"{value.GetType()}\"");
	    // if (targetType != typeof(bool?))
	    //     return ThrowHelper.ThrowArgumentException<object>(nameof(targetType), $"Expected target type \"{typeof(bool?)}\" but got \"{targetType}\"");
	    // bool? result = tagActivationCondition switch
	    // {
	    //     ActivationCondition.None => false,
	    //     ActivationCondition.IsShooting => true,
	    //     ActivationCondition.IsNotShooting => null,
	    //     _ => ThrowHelper.ThrowArgumentOutOfRangeException<bool>(
	    //         nameof(tagActivationCondition),
	    //         tagActivationCondition,
	    //         "Unknown item class activation condition")
	    // };
	    // _logger.Debug("Converted \"{Value}\" to \"{Result}\"", tagActivationCondition, result);
	    // return result;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
	    throw new NotImplementedException();
	    // if (targetType != typeof(ActivationCondition))
	    //     return ThrowHelper.ThrowArgumentException<object>(nameof(targetType), $"Expected target type \"{typeof(ActivationCondition)}\" but got \"{targetType}\"");
	    // var result = value switch
	    // {
	    //     true => ActivationCondition.IsShooting,
	    //     false => ActivationCondition.None,
	    //     null => ActivationCondition.IsNotShooting,
	    //     _ => ThrowHelper.ThrowArgumentException<ActivationCondition>(
	    //         nameof(value), $"Expected value of type \"{typeof(bool)}\", but got \"{value}\" of type \"{value.GetType()}\"")
	    // };
	    // _logger.Debug("Converted \"{Value}\" to \"{Result}\"", value, result);
	    // return result;
    }
    
    private readonly ILogger _logger;
}