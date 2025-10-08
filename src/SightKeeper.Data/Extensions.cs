using SightKeeper.Domain;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static T GetInnermost<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>().Last();
	}

	public static T Get<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>().First();
	}

	private static IEnumerable<object> GetSelfAndChildren(this object obj)
	{
		yield return obj;
		while (obj is Decorator<object> decorator)
		{
			obj = decorator.Inner;
			yield return obj;
		}
	}
}