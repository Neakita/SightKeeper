using SightKeeper.Abstractions.Domain;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public interface IModelToVMStrategy<TModelVM, TModelEntity> where TModelVM : class, IModelVM<TModelEntity> where TModelEntity : class, IModel
{
	TModelVM ConvertToVM(TModelEntity model);
}