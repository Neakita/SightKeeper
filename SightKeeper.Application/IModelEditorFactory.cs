using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public interface IModelEditorFactory
{
	public ModelEditor Create(Model model);
}
