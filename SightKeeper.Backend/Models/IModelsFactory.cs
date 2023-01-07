using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.Backend.Models;

public interface IModelsFactory<TModel> where TModel : Model
{
	public TModel Create(string name, Resolution resolution);
}
