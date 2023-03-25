using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Domain.Model.Common;

[Owned]
public class Resolution
{
	public Resolution(ushort width = 320, ushort height = 320)
	{
		Width = width;
		Height = height;
	}

	public ushort Width { get; set; }

	public ushort Height { get; set; }
}