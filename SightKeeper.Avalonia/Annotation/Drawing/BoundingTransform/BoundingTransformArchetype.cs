using System;
using System.Collections.Generic;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class BoundingTransformArchetype
{
	public static BoundingTransformArchetype Create(Side changingSide)
	{
		var sides = Scroll(Sides, GetIndex(changingSide));
		return new BoundingTransformArchetype(sides);
	}

	private static List<Side> Scroll(IReadOnlyList<Side> sides, int indexes)
	{
		List<Side> result = new(4);
		for (int i = 0; i < 4; i++)
		{
			var sourceIndex = (i + indexes) % 4;
			if (sourceIndex < 0)
				sourceIndex += 4;
			result.Add(sides[sourceIndex]);
		}

		return result;
	}

	private static IReadOnlyList<Side> Sides { get; } = [Side.Left, Side.Top, Side.Right, Side.Bottom];

	private static IReadOnlyList<Func<Bounding, double>> SideGetters { get; } =
	[
		bounding => bounding.Left,
		bounding => bounding.Top,
		bounding => bounding.Right,
		bounding => bounding.Bottom
	];

	private static IReadOnlyList<Func<Bounding, double, Bounding>> SideSetters { get; } =
	[
		(bounding, value) => bounding with { Left = value },
		(bounding, value) => bounding with { Top = value },
		(bounding, value) => bounding with { Right = value },
		(bounding, value) => bounding with { Bottom = value }
	];

	internal static Func<Bounding, double> GetSideGetter(Side side)
	{
		var index = GetIndex(side);
		return SideGetters[index];
	}

	private static Func<Bounding, double, Bounding> GetSideSetter(Side side)
	{
		var index = GetIndex(side);
		return SideSetters[index];
	}

	private static byte GetIndex(Side side)
	{
		return (byte)side;
	}

	private static bool IsNearSide(Side side)
	{
		return side <= Side.Top; // matches 2 first enum values, i.e. Left and Top
	}

	private static BoundingSideAccessor CreateAccessor(Side side)
	{
		var getter = GetSideGetter(side);
		var setter = GetSideSetter(side);
		return new BoundingSideAccessor(getter, setter);
	}

	public double GetVectorValue(Vector2<double> vector)
	{
		return _vectorValueGetter(vector);
	}

	public Bounding AddToChangingSide(Bounding bounding, double delta, double minimumSize)
	{
		var value = _changingBoundingSideAccessor.GetValue(bounding);
		value += delta;
		value = Clamp(bounding, value, minimumSize);
		return _changingBoundingSideAccessor.SetValue(bounding, value);
	}

	private readonly Side _changingSide;
	private readonly BoundingSideAccessor _changingBoundingSideAccessor;
	private readonly BoundingSideAccessor _oppositeBoundingSideAccessor;
	private readonly Func<Vector2<double>, double> _vectorValueGetter;

	private double Clamp(Bounding bounding, double value, double minimumSize)
	{
		var oppositeSideValue = _oppositeBoundingSideAccessor.GetValue(bounding);
		return IsNearSide(_changingSide)
			? Math.Clamp(value, 0, oppositeSideValue - minimumSize)
			: Math.Clamp(value, oppositeSideValue + minimumSize, 1);
	}

	private BoundingTransformArchetype(List<Side> sides)
	{
		_changingSide = sides[0];
		_changingBoundingSideAccessor = CreateAccessor(_changingSide);
		_oppositeBoundingSideAccessor = CreateAccessor(sides[2]);
		_vectorValueGetter = GetIndex(_changingSide) % 2 == 0 ? vector => vector.X : vector => vector.Y;
	}
}