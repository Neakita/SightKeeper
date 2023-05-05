namespace SightKeeper.Application;

public interface ModelEditor : IDisposable, IAsyncDisposable
{
	void SaveChanges();
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
	void DiscardChanges();
	Task DiscardChangesAsync(CancellationToken cancellationToken = default);
}