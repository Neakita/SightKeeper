using SightKeeper.DAL;

namespace SightKeeper.Backend.Abstract;

public interface IDbFactory<T> where T : IDbContext
{
	T NewContext { get; }
}