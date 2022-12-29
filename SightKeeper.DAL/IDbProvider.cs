using Microsoft.EntityFrameworkCore;

namespace SightKeeper.DAL;

public interface IDbProvider<T> where T : DbContext
{
	T NewContext { get; }
}