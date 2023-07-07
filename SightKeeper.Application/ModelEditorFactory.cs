using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ModelEditorFactory
{
	ModelEditor BeginEdit(Model model);
	Task<ModelEditor> BeginEditAsync(Model model, CancellationToken cancellationToken = default);
}