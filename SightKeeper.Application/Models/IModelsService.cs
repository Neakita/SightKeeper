using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Models;

public interface IModelsService<TModel> where TModel : Model
{
	TModel Create(string name, Resolution resolution);

	void Add(TModel model);

	void Delete(TModel model);

	void Delete(IEnumerable<TModel> models);
}