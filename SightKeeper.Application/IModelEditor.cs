using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public interface IModelEditor
{
	Model EditableModel { get; set; }
	void SaveChanges();
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
	void DiscardChanges();
	Task DiscardChangesAsync(CancellationToken cancellationToken = default);
}