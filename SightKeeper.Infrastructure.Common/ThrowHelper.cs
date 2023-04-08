namespace SightKeeper.Infrastructure.Common;

public static class ThrowHelper
{
	public static void ThrowIfNull<T>(this T? argument, string argumentName)
	{
		if (argument == null) throw new ArgumentNullException(argumentName);
	}

	public static void ThrowIf(bool condition, string message)
	{
		if (condition) throw new InvalidOperationException(message);
	}
}
