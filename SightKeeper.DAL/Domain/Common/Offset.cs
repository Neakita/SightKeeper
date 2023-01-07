using Microsoft.EntityFrameworkCore;

namespace SightKeeper.DAL.Domain.Common;

[Owned]
public sealed class Offset
{
	public Offset(float horizontal = 0, float vertical = 0)
	{
		Horizontal = horizontal;
		Vertical = vertical;
	}
	
	
	public float Horizontal { get; set; }
	public float Vertical { get; set; }
}