namespace SightKeeper.Abstractions;

public interface IDbProvider<T> where T : IDbContext
{
	T NewContext { get; }
}