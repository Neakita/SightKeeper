using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public interface IModelsListVM<TModelVM, TModelEntity>
	where TModelVM : IModelVM<TModelEntity> where TModelEntity : Model
{
	IEnumerable<TModelVM> Models { get; }


	ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	ReactiveCommand<TModelVM, Unit> DeleteModelCommand { get; }
}