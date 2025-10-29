namespace SightKeeper.Domain;

public static class Extensions
{
	public static T GetInnermost<T>(this object obj)
	{
		return obj.Get<T>().Last();
	}

	public static T GetFirst<T>(this object obj)
	{
		return obj.Get<T>().First();
	}

	public static IEnumerable<T> Get<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>();
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