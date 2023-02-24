using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Backend;

public interface IModelEditorFactory
{
	public IModelEditor Create(Model model);
}
