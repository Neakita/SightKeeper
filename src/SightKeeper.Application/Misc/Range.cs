namespace SightKeeper.Application.Misc;

internal readonly struct Range
{
	public static Range FromCount(int start, int count)
	{
		var end = start + count - 1;
		return new Range(start, end);
	}

	public int Start { get; }
	public int End { get; }
	public int Count => End - Start + 1;

	public Range(int start, int end)
	{
		Start = start;
		End = end;
	}

	public override string ToString()
	{
		if (Count == 1)
			return $"{Start}";
		return $"{Start} ~ {End} ({Count})";
	}
}