using SightKeeper.Domain.Model.Abstract;
using Splat;

namespace SightKeeper.Backend;

public sealed class ModelEditorFactory : IModelEditorFactory
{
	public IModelEditor Create(Model model)
	{
		IModelEditor editor = Locator.Current.GetService<IModelEditor>() ??
		                      throw new ServiceNotFoundException(typeof(IModelEditor));
		editor.EditableModel = model;
		return editor;
	}
}
