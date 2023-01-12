using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Persistance;

public interface IDbProvider<T> where T : DbContext
{
	T NewContext { get; }
}