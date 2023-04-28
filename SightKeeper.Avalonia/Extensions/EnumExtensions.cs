using System;
using System.Collections.Generic;
using System.Linq;

namespace SightKeeper.Avalonia.Extensions;

public static class EnumExtensions
{
	public static bool IsFlagSet<T>(this T value, T flag) where T : struct, Enum
	{
		long lValue = Convert.ToInt64(value);
		long lFlag = Convert.ToInt64(flag);
		return (lValue & lFlag) != 0;
	}

	public static IReadOnlyCollection<T> GetFlags<T>(this T value) where T : struct, Enum =>
		Enum.GetValues(typeof(T)).Cast<T>().Where(flag => value.IsFlagSet(flag)).ToList();
}
