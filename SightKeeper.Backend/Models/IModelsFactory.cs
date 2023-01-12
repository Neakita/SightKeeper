using SightKeeper.Domain.Abstract;
using SightKeeper.Domain.Common;

namespace SightKeeper.Backend.Models;

public interface IModelsFactory<TModel> where TModel : Model
{
	public TModel Create(string name, Resolution resolution);
}
