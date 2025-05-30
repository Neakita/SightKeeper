namespace SightKeeper.Data.Services;

public interface IdRepository<in T> : ReadIdRepository<T>, WriteIdRepository<T>;