using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia;

internal sealed class ExponentialNumericUpDownIncrementBehavior : Behavior<NumericUpDown>
{
	public static readonly StyledProperty<decimal> IncrementProperty =
		NumericUpDown.IncrementProperty.AddOwner<ExponentialNumericUpDownIncrementBehavior>();

	public decimal Increment
	{
		get => GetValue(IncrementProperty);
		set => SetValue(IncrementProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.ValueChanged += OnAssociatedObjectValueChanged;
		AssociatedObject.Loaded += OnAssociatedObjectLoaded;
	}

	private void OnAssociatedObjectLoaded(object? sender, RoutedEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		var lookup = AssociatedObject
			.GetVisualDescendants()
			.OfType<RepeatButton>()
			.Where(button => button.Name != null)
			.ToLookup(button => button.Name);
		_decreaseButton = lookup["PART_DecreaseButton"].Single();
		_increaseButton = lookup["PART_IncreaseButton"].Single();
		_decreaseButton.PointerEntered += OnPointerOverDecreaseButton;
		_increaseButton.PointerEntered += OnPointerOverIncreaseButton;
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.ValueChanged -= OnAssociatedObjectValueChanged;
		AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
	}

	private RepeatButton? _decreaseButton;
	private RepeatButton? _increaseButton;
	private bool _forward;

	private void OnPointerOverDecreaseButton(object? sender, PointerEventArgs e)
	{
		_forward = false;
		UpdateIncrement();
	}

	private void OnPointerOverIncreaseButton(object? sender, PointerEventArgs e)
	{
		_forward = true;
		UpdateIncrement();
	}

	private void OnAssociatedObjectValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
	{
		UpdateIncrement();
	}

	private void UpdateIncrement()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Increment = GetIncrementFromValue(AssociatedObject.Value);
	}

	private decimal GetIncrementFromValue(decimal? value)
	{
		if (value is null)
			return Increment;
		if (_forward)
		{
			var target = GetUpperTarget(value.Value);
			return target - value.Value;
		}
		else
		{
			var target = GetLowerTarget(value.Value);
			return value.Value - target;
		}
	}

	private decimal GetLowerTarget(decimal value)
	{
		if (value > 1)
			return value - Increment;
		decimal result = Increment;
		while (result >= value)
			result /= 10;
		return result;
	}

	private decimal GetUpperTarget(decimal value)
	{
		if (value >= 1)
			return value + Increment;
		decimal result = Increment;
		while (result > value)
			result /= 10;
		result *= 10;
		return result;
	}
}