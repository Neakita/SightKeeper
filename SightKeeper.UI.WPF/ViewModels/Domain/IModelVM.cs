using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public interface IModelVM
{
	string Name { get; set; }
	
	
	public Model Model { get; }
}

public interface IModelVM<TModel> : IModelVM where TModel : Model
{
	new TModel Model { get; }
}