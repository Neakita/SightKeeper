namespace SightKeeper.Domain.Model.Common;

public sealed class Resolution
{
	public ushort Width => _width;
	public ushort Height => _height;
	
	public Resolution()
	{
		_width = 320;
		_height = 320;
	}
	
	public Resolution(ushort width, ushort height)
	{
		_width = width;
		_height = height;
	}

	public override string ToString() => $"{Width}x{Height}";

	public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Resolution other && Equals(other);

	private bool Equals(Resolution other) => Width == other.Width && Height == other.Height;

	public override int GetHashCode() => HashCode.Combine(_width, _height);

	private readonly ushort _width;
	private readonly ushort _height;
}