using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public interface ModelEditor
{
	void SaveChanges(Model model);
	Task SaveChangesAsync(Model model, CancellationToken cancellationToken = default);
	void RollbackChanges(Model model);
	Task RollbackChangesAsync(Model model, CancellationToken cancellationToken = default);
}