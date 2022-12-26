using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Abstractions;

public interface IModelsProvider<TModel> where TModel : class, IModel
{
	IEnumerable<TModel> Models { get; }
}