using SightKeeper.Application;

namespace SightKeeper.Data.Services;

internal interface Repository<T> :
	ReadRepository<T>,
	ObservableRepository<T>,
	WriteRepository<T>,
	IDisposable;