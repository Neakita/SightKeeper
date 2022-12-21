using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models.Abstract;

public interface IModelsProvider
{
	IEnumerable<Model> Models { get; }

	void EnsureDataPopulated(Model model);
}