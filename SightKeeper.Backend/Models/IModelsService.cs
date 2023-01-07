using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.Backend.Models;

public interface IModelsService<TModel> where TModel : Model
{
	TModel Create(string name, Resolution resolution);

	void Add(TModel model);

	void Delete(TModel model);

	void Delete(IEnumerable<TModel> models);
}