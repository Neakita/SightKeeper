using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SightKeeper.UI.WPF.ViewModels.Domain;

namespace SightKeeper.UI.WPF.ViewModels.Elements;

public interface IModelsList
{
	string Label { get; }
	
	IEnumerable<IModelVM> Models { get; }
	
	ReactiveCommand<Unit, Unit> CreateNewModelCommand { get; }
	ReactiveCommand<ModelVM, Unit> DeleteModelCommand { get; }
}

public interface IModelsList<TModelVM> : IModelsList where TModelVM : ModelVM
{
	
}