using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Abstractions;

public interface IModelsService<TModel> where TModel : class, IModel
{
	TModel Create(string name, ushort width, ushort height);

	void Delete(TModel model);

	void Delete(IEnumerable<TModel> models);
}