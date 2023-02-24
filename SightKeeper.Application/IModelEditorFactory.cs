using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public interface IModelEditorFactory
{
	public IModelEditor Create(Model model);
}
