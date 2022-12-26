using SightKeeper.Abstractions.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public interface IModelVM<TModel> where TModel : IModel
{
	TModel Model { get; }

	string Name { get; }
}