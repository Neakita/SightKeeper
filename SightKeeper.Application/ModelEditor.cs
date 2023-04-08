namespace SightKeeper.Application;

public interface ModelEditor : IDisposable, IAsyncDisposable
{
	void SaveChanges();
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
	void RollbackChanges();
	Task RollbackChangesAsync(CancellationToken cancellationToken = default);
}