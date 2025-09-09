namespace SightKeeper.Domain;

public static class DecoratorExtensions
{
	public static T GetInnermost<T>(this object obj)
	{
		return obj.GetSelfAndChildren().OfType<T>().Last();
	}

	public static IEnumerable<object> GetSelfAndChildren(this object obj)
	{
		yield return obj;
		while (obj is Decorator<object> decorator)
		{
			obj = decorator.Inner;
			yield return obj;
		}
	}
}