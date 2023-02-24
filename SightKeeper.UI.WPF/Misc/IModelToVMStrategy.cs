using SightKeeper.Domain.Model.Abstract;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.Misc;

public interface IModelToVMStrategy<TModelVM, TModelEntity> where TModelVM : class, IModelVM<TModelEntity>
	where TModelEntity : Model
{
	TModelVM CreateVM(TModelEntity model);
}