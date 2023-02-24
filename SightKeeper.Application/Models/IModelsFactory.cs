using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Models;

public interface IModelsFactory<TModel> where TModel : Model
{
	public TModel Create(string name, Resolution resolution);
}
