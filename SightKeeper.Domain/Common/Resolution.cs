using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Domain.Common;

[Owned]
public class Resolution
{
	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;
	}

	public ushort Width { get; private set; }

	public ushort Height { get; private set; }
}