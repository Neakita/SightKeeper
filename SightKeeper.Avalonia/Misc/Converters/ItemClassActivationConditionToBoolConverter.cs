using System;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class ItemClassActivationConditionToBoolConverter : IValueConverter
{
    public ItemClassActivationConditionToBoolConverter()
    {
        _logger = Log.ForContext<ItemClassActivationConditionToBoolConverter>();
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        if (value is not ItemClassActivationCondition itemClassActivationCondition)
            return ThrowHelper.ThrowArgumentException<object>(nameof(value), $"Expected an \"{typeof(ItemClassActivationCondition)}\", but got \"{value}\" of type \"{value.GetType()}\"");
        if (targetType != typeof(bool?))
            return ThrowHelper.ThrowArgumentException<object>(nameof(targetType), $"Expected target type \"{typeof(bool?)}\" but got \"{targetType}\"");
        bool? result = itemClassActivationCondition switch
        {
            ItemClassActivationCondition.None => false,
            ItemClassActivationCondition.IsShooting => true,
            ItemClassActivationCondition.IsNotShooting => null,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<bool>(
                nameof(itemClassActivationCondition),
                itemClassActivationCondition,
                "Unknown item class activation condition")
        };
        _logger.Debug("Converted \"{Value}\" to \"{Result}\"", itemClassActivationCondition, result);
        return result;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(ItemClassActivationCondition))
            return ThrowHelper.ThrowArgumentException<object>(nameof(targetType), $"Expected target type \"{typeof(ItemClassActivationCondition)}\" but got \"{targetType}\"");
        var result = value switch
        {
            true => ItemClassActivationCondition.IsShooting,
            false => ItemClassActivationCondition.None,
            null => ItemClassActivationCondition.IsNotShooting,
            _ => ThrowHelper.ThrowArgumentException<ItemClassActivationCondition>(
                nameof(value), $"Expected value of type \"{typeof(bool)}\", but got \"{value}\" of type \"{value.GetType()}\"")
        };
        _logger.Debug("Converted \"{Value}\" to \"{Result}\"", value, result);
        return result;
    }
    
    private readonly ILogger _logger;
}