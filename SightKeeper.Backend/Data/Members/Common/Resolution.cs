using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Backend.Data.Members.Common;

[Owned]
public sealed class Resolution
{
	public Guid Id { get; set; }
	public ushort Width { get; set; } = 320;
	public ushort Height { get; set; } = 320;

	public Resolution() { }
	
	public Resolution(ushort width, ushort height)
	{
		Width = width;
		Height = height;
	}
}
