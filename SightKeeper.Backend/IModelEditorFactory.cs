using SightKeeper.Domain.Abstract;

namespace SightKeeper.Backend;

public interface IModelEditorFactory
{
	public IModelEditor Create(Model model);
}
