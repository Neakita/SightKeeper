using System;
using System.Collections.Generic;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

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

	private static IReadOnlyList<Func<Bounding, double, Bounding>> SideTransforms { get; } =
	[
		(bounding, value) => bounding with { Left = value },
		(bounding, value) => bounding with { Top = value },
		(bounding, value) => bounding with { Right = value },
		(bounding, value) => bounding with { Bottom = value }
	];

	private static Func<Bounding, double> GetSideGetter(Side side)
	{
		var index = GetIndex(side);
		return SideGetters[index];
	}

	private static Func<Bounding, double, Bounding> GetSideSetter(Side side)
	{
		var index = GetIndex(side);
		return SideTransforms[index];
	}

	private static byte GetIndex(Side side)
	{
		return (byte)side;
	}

	public static double GetSide(Bounding bounding, Side side)
	{
		return GetSideGetter(side).Invoke(bounding);
	}

	public static Bounding SetSide(Bounding bounding, Side side, double value)
	{
		return GetSideSetter(side).Invoke(bounding, value);
	}

	public static double GetVectorValue(Side side, Vector2<double> vector)
	{
		return GetIndex(side) % 2 == 0 ? vector.X : vector.Y;
	}

	public static bool IsOpposite(Side side)
	{
		return side > Side.Top;
	}

	public Bounding AddToChangingSide(Bounding bounding, double delta, double minimumSize)
	{
		var value = GetSideGetter(ChangingSide).Invoke(bounding);
		value += delta;
		value = Clamp(bounding, value, minimumSize);
		return GetSideSetter(ChangingSide).Invoke(bounding, value);
	}

	public Side ChangingSide { get; }
	public Side PerpendicularSide1 { get; }
	public Side OppositeSide { get; }
	public Side PerpendicularSide2 { get; }

	public double Clamp(Bounding bounding, double value, double minimumSize)
	{
		if (!IsOpposite(ChangingSide))
		{
			var oppositeSide = GetSideGetter(OppositeSide).Invoke(bounding);
			var clamp = Math.Clamp(value, 0, oppositeSide - minimumSize);
			return clamp;
		}
		return Math.Clamp(value, GetSideGetter(OppositeSide).Invoke(bounding) + minimumSize, 1);
	}
	
	private BoundingTransformArchetype(List<Side> sides)
	{
		ChangingSide = sides[0];
		PerpendicularSide1 = sides[1];
		OppositeSide = sides[2];
		PerpendicularSide2 = sides[3];
	}
}