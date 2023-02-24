using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Infrastructure.Data;

public interface IDbProvider<T> where T : DbContext
{
	T NewContext { get; }
}