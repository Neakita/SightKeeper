using SightKeeper.DAL;

namespace SightKeeper.Abstractions;

public interface IAppDbProvider : IDbProvider<IAppDbContext> { }
