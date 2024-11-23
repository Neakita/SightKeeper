using System;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class BoundingSideAccessor
{
	public BoundingSideAccessor(Func<Bounding, double> getter, Func<Bounding, double, Bounding> setter)
	{
		_getter = getter;
		_setter = setter;
	}

	public double GetValue(Bounding bounding)
	{
		return _getter(bounding);
	}

	public Bounding SetValue(Bounding bounding, double value)
	{
		return _setter(bounding, value);
	}

	private readonly Func<Bounding, double> _getter;
	private readonly Func<Bounding, double, Bounding> _setter;
}