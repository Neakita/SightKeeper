namespace SightKeeper.Domain.Model.Common;

public sealed class Resolution
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	
	public Resolution()
	{
		Width = 320;
		Height = 320;
	}
	
	public Resolution(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public override string ToString() => $"{Width}x{Height}";

	private bool Equals(Resolution other)
	{
		return Width == other.Width && Height == other.Height;
	}

	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is Resolution other && Equals(other);
	}

	public override int GetHashCode() => HashCode.Combine(Width, Height);
}