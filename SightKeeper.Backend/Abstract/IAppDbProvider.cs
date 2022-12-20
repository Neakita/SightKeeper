using SightKeeper.DAL;

namespace SightKeeper.Backend.Abstract;

public interface IAppDbProvider : IDbProvider<IAppDbContext> { }
