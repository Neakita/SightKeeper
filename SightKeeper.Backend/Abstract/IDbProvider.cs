using SightKeeper.DAL;

namespace SightKeeper.Backend.Abstract;

public interface IDbProvider<T> where T : IDbContext
{
	T NewContext { get; }
}