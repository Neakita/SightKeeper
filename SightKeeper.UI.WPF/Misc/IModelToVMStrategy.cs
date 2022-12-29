using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public interface IModelToVMStrategy<TModelVM, TModelEntity> where TModelVM : class, IModelVM<TModelEntity> where TModelEntity : Model
{
	TModelVM ConvertToVM(TModelEntity model);
}