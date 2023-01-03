using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.Backend;

public interface IModelEditor
{
	Model EditableModel { get; set; }
	void SaveChanges();
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
	void DiscardChanges();
	Task DiscardChangesAsync(CancellationToken cancellationToken = default);
}