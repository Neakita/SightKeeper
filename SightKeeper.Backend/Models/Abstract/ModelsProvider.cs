using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models.Abstract;

public interface ModelsProvider
{
	IEnumerable<Model> Models { get; }

	void EnsureDataPopulated(Model model);
}