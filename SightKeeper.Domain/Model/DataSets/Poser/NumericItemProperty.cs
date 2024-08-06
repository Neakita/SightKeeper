using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class NumericItemProperty : ItemProperty
{
	public double MinimumValue
	{
		get => _minimumValue;
		set
		{
			Guard.IsEmpty(_tag.Items);
			Guard.IsLessThan(value, MaximumValue);
			_minimumValue = value;
		}
	}

	public double MaximumValue
	{
		get => _maximumValue;
		set
		{
			Guard.IsEmpty(_tag.Items);
			Guard.IsGreaterThan(value, MinimumValue);
			_maximumValue = value;
		}
	}

	public double Range => MaximumValue - MinimumValue;

	public NumericItemProperty(PoserTag tag, string name, double minimumValue, double maximumValue) : base(name)
	{
		_tag = tag;
		SetBounds(minimumValue, maximumValue);
	}

	public void SetBounds(double minimumValue, double maximumValue)
	{
		Guard.IsEmpty(_tag.Items);
		Guard.IsLessThan(minimumValue, maximumValue);
		_minimumValue = minimumValue;
		_maximumValue = maximumValue;
	}

	public double GetRangedValue(double normalizedValue)
	{
		Guard.IsBetweenOrEqualTo(normalizedValue, 0, 1);
		return MinimumValue + Range * normalizedValue;
	}

	public double GetNormalizedValue(double rangedValue)
	{
		Guard.IsBetweenOrEqualTo(rangedValue, MinimumValue, MaximumValue);
		return (rangedValue - MinimumValue) / Range;
	}

	private readonly PoserTag _tag;
	private double _minimumValue;
	private double _maximumValue;
}