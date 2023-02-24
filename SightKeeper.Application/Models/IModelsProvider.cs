using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Models;

public interface IModelsProvider<TModel> where TModel : Model
{
	IEnumerable<TModel> Models { get; }
}