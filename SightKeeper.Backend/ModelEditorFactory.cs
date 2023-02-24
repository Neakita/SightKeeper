using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.Backend;

public sealed class ModelEditorFactory : IModelEditorFactory
{
	public IModelEditor Create(Model model)
	{
		IModelEditor editor = Locator.Resolve<IModelEditor>();
		editor.EditableModel = model;
		return editor;
	}
}
