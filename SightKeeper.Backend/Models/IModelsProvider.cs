
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.Backend.Models;

public interface IModelsProvider<TModel> where TModel : Model
{
	IEnumerable<TModel> Models { get; }
}