using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models.Abstract;

public interface IModelsService
{
	void AddModel(Model model);
	Task AddModelAsync(Model model, CancellationToken cancellationToken = default);
	void DeleteModel(Model model);
	Task DeleteModelAsync(Model model, CancellationToken cancellationToken = default);
	void DeleteModels(IEnumerable<Model> models);
	Task DeleteModelsAsync(IEnumerable<Model> models, CancellationToken cancellationToken = default);
}