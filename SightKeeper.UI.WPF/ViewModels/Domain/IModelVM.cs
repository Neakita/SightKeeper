using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public interface IModelVM<TModel> where TModel : Model
{
	TModel Model { get; }

	string Name { get; }
}