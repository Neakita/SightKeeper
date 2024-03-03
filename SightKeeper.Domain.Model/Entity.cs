using FlakeId;

namespace SightKeeper.Domain.Model;

public abstract class Entity
{
	// used by EF Core
#pragma warning disable CS0169 // Field is never used
	private readonly Id _id;
#pragma warning restore CS0169 // Field is never used
}