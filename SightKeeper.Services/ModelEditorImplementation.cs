using SightKeeper.Application;
using SightKeeper.Data;

namespace SightKeeper.Services;

public sealed class ModelEditorImplementation : ModelEditor
{
	public ModelEditorImplementation(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void SaveChanges() => _dbContext.SaveChanges();
	public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
		await _dbContext.SaveChangesAsync(cancellationToken);

	public void Dispose()
	{
		_dbContext.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		await _dbContext.DisposeAsync();
	}

	private readonly AppDbContext _dbContext;
}
