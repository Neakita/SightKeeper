namespace SightKeeper.Domain;

public static class DecoratorExtensions
{
	public static T UnWrapDecorator<T>(this object obj)
	{
		return obj switch
		{
			T result => result,
			Decorator<object> decorator => UnWrapDecorator<T>(decorator.Inner),
			_ => throw new ArgumentOutOfRangeException(nameof(obj), obj, null)
		};
	}
}