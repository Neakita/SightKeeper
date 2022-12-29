using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Backend.Models;

public interface IModelsProvider<TModel> where TModel : class, IModel
{
	IEnumerable<TModel> Models { get; }
}