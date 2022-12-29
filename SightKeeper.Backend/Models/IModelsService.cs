using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.Backend.Models;

public interface IModelsService<TModel> where TModel : Model
{
	TModel Create(string name, ushort width, ushort height);

	void Delete(TModel model);

	void Delete(IEnumerable<TModel> models);
}